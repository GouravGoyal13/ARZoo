using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using System.Linq;

[XmlType("Row")]
public class LevelData
{

    [XmlElement("levelID")] public string LevelID{ get ; set; }

    [XmlElement("levelFinishTimeInSeconds")] public int LevelFinishTimeInSeconds{ get; set; }

    [XmlElement("levelCategory")] public string LevelCategorys{ get; set; }

    [XmlElement("rowSize")] public int RowSizes{ get; set; }

    [XmlElement("columnSize")] public int ColumnSizes{ get; set; }

    [XmlElement("playData")] public string PlayData{ get; set; }

    [XmlElement("delayBetweenMoves")] public float DelayBetweenMoves{ get; set; }


    public int[] TileData
    {
        get { return PlayData.Split(',').Select(int.Parse).ToArray(); }
    }

}
