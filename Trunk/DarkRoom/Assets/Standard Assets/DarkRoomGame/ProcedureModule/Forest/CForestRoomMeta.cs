using System;
using System.Collections.Generic;
using System.Xml;
using DarkRoom.Game;
using UnityEngine;

namespace DarkRoom.PCG
{
    public class CForestRoomMeta : CBaseMeta
    {
        public enum TileType
        {
            Floor, //可通行的地面
            Wall, //墙壁或者柱子
            Exit //出口
        }

        /// <summary>
        /// 是否有推荐位置
        /// </summary>
        public Vector2Int PreferLocation;

        public string Name;

        /// <summary>
        /// 是否唯一的
        /// </summary>
        public bool Unique;

        /// <summary>
        /// 是否有边界. 就是占用的空间要比实际的尺寸大一圈
        /// 比如最外围是一些围墙, 然后我们扩展一圈
        /// 
        /// 但其实, 根本不需要. 我手动放大即可
        /// </summary>
        public bool Border;

        /// <summary>
        /// 是否有隧道
        /// </summary>
        public bool NoTunnels;

        /// <summary>
        /// 是否特殊?
        /// </summary>
        public bool Special;

        /// <summary>
        /// 可通行的地方 符号 .
        /// </summary>
        public TileType[,] Spots;

        private Vector2Int m_size;

        /// <summary>
        /// 是否有推荐位置
        /// </summary>
        public bool HasPreferLocation => PreferLocation.x <= 0 || PreferLocation.y <= 0;

        /// <summary>
        /// 房子的尺寸
        /// </summary>
        public Vector2Int Size => m_size;

        public CForestRoomMeta(string idKey) : base(idKey) { }

        public void SetSize(int cols, int rows)
        {
            Spots = new TileType[cols, rows];
            m_size = new Vector2Int(cols, rows);
        }

        public void SetSpot(int col, int row, char v)
        {
            switch (v)
            {
                case '.':
                    SetSpot(col, row, TileType.Floor);
                    break;
                case '!':
                    SetSpot(col, row, TileType.Exit);
                    break;
                case '#':
                    SetSpot(col, row, TileType.Wall);
                    break;
            }
        }

        public void SetSpot(int col, int row, TileType v)
        {
            bool b = ValidSpot(col, row);
            if (!b)
            {
                Debug.LogWarningFormat("Check Room mapSize and Shape. col={0} row={1}", col, row);
                return;
            }

            Spots[col, row] = v;
        }

        public TileType GetSpot(int col, int row)
        {
            bool b = ValidSpot(col, row);
            if (!b)
            {
                Debug.LogWarningFormat("Check Room mapSize and Shape. col={0} row={1}", col, row);
                return TileType.Floor;
            }

            return Spots[col, row];
        }

        private bool ValidSpot(int col, int row)
        {
            if (col < 0 || row < 0) return false;
            if (col > m_size.x || row > m_size.y) return false;
            return true;
        }
    }

    public class CForestRoomMetaManager
    {
        private static readonly Dictionary<string, CForestRoomMeta> m_dict = new Dictionary<string, CForestRoomMeta>();

        public static void AddMeta(CForestRoomMeta meta)
        {
            if (m_dict.ContainsKey(meta.Id))
            {
                Debug.LogError(string.Format("CForestRoomMetaManager ALREADY CONTAIN the meta with id -- {0} ", meta.Id));
            }

            m_dict[meta.Id] = meta;
        }

        public static CForestRoomMeta GetMeta(string id)
        {
            CForestRoomMeta meta = null;
            bool v = m_dict.TryGetValue(id, out meta);
            if (!v) Debug.LogError(string.Format("CForestRoomMetaManager DO NOT CONTAIN the meta with id -- {0} ", id));
            return meta;
        }
    }

    public class CForestRoomMetaParser : CMetaParser
    {
        public CForestRoomMetaParser() : base(true)
        {
        }

        public override void Execute(string content)
        {
            base.Execute(content);
            m_xreader.ReadRootNode();

            foreach (XmlElement node in m_xreader.rootChildNodes)
            {
                CForestRoomMeta meta = new CForestRoomMeta(node.GetAttribute("id"));
                meta.NameKey = node.GetAttribute("name");
                m_xreader.TryReadChildNodeAttr(node, "PreferLocation", ref meta.PreferLocation);

                int cols = 0, rows = 0;
                m_xreader.TryReadChildNodeAttr(node, "Shape", "x", ref cols);
                m_xreader.TryReadChildNodeAttr(node, "Shape", "y", ref rows);
                meta.SetSize(cols, rows);

                List<string> lines = new List<string>();
                m_xreader.TryReadChildNodesAttr(node, "Shape", "v", lines);
                for (int r = 0; r < lines.Count; r++)
                {
                    var arr = lines[r].ToCharArray();
                    for (int c = 0; c < arr.Length; c++)
                    {
                        meta.SetSpot(c, r, arr[c]);
                    }
                }

                CForestRoomMetaManager.AddMeta(meta);

            }
        }
    }
}
