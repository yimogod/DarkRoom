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
            None = -1,
            Floor, //可通行的地面 .
            Wall, //墙壁或者柱子 #
            Exit, //出口 !
            Door, //$
            InnerRoad, //房屋内部道路 ^
            OuterRoad, //连接房间的道路, 没有符号. 我们在算法里面使用
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
        /// 可通行的地方 符号 .
        /// </summary>
        public TileType[,] Spots;

        /// <summary>
        /// 房子的门的列表
        /// </summary>
        public List<Vector2Int> DoorPosList = new List<Vector2Int>();

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

        /// <summary>
        /// 我们的门是否连着路, 如果连着我们用路来标记.否则我们用floor来标记
        /// </summary>
        public bool IsDoorNextToInnerRoad(int index)
        {
            var pos = DoorPosList[index];

            var next = pos + Vector2Int.up;
            var type = GetSpot(next.x, next.y);
            if (type == TileType.InnerRoad) return true;

            next = pos + Vector2Int.down;
            type = GetSpot(next.x, next.y);
            if (type == TileType.InnerRoad) return true;

            next = pos + Vector2Int.left;
            type = GetSpot(next.x, next.y);
            if (type == TileType.InnerRoad) return true;

            next = pos + Vector2Int.right;
            type = GetSpot(next.x, next.y);
            if (type == TileType.InnerRoad) return true;

            return false;
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
                case '$':
                    SetSpot(col, row, TileType.Door);
                    DoorPosList.Add(new Vector2Int(col, row));
                    break;
                case '^':
                    SetSpot(col, row, TileType.InnerRoad);
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
