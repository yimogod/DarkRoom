using System;
using System.Collections.Generic;
using System.Xml;
using DarkRoom.Game;
using UnityEngine;

namespace DarkRoom.PCG
{
    public class CForestRoomMeta : CBaseMeta
    {
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
        public CForestRoomTileType[,] Spots;

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
            Spots = new CForestRoomTileType[cols, rows];
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
            if (type == CForestRoomTileType.InnerRoad) return true;

            next = pos + Vector2Int.down;
            type = GetSpot(next.x, next.y);
            if (type == CForestRoomTileType.InnerRoad) return true;

            next = pos + Vector2Int.left;
            type = GetSpot(next.x, next.y);
            if (type == CForestRoomTileType.InnerRoad) return true;

            next = pos + Vector2Int.right;
            type = GetSpot(next.x, next.y);
            if (type == CForestRoomTileType.InnerRoad) return true;

            return false;
        }

        public void SetSpot(int col, int row, char v)
        {
            switch (v)
            {
                case '.':
                    SetSpot(col, row, CForestRoomTileType.Floor);
                    break;
                case '!':
                    SetSpot(col, row, CForestRoomTileType.Exit);
                    break;
                case '#':
                    SetSpot(col, row, CForestRoomTileType.Wall);
                    break;
                case '$':
                    SetSpot(col, row, CForestRoomTileType.Door);
                    DoorPosList.Add(new Vector2Int(col, row));
                    break;
                case '^':
                    SetSpot(col, row, CForestRoomTileType.InnerRoad);
                    break;
            }
        }

        public void SetSpot(int col, int row, CForestRoomTileType v)
        {
            bool b = ValidSpot(col, row);
            if (!b)
            {
                Debug.LogWarningFormat("Check Room mapSize and Shape. col={0} row={1}", col, row);
                return;
            }

            Spots[col, row] = v;
        }

        public CForestRoomTileType GetSpot(int col, int row)
        {
            bool b = ValidSpot(col, row);
            if (!b)
            {
                Debug.LogWarningFormat("Check Room mapSize and Shape. col={0} row={1}", col, row);
                return CForestRoomTileType.Floor;
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
            if (m_dict.ContainsKey(meta.sId))
            {
                Debug.LogError(string.Format("CForestRoomMetaManager ALREADY CONTAIN the meta with id -- {0} ", meta.sId));
            }

            m_dict[meta.sId] = meta;
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
                var shapeNode = node.SelectSingleNode("Shape");
                m_xreader.TryReadChildNodesAttr(shapeNode, "Line", lines);
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
