using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatCode_Selenium
{
    public class Clause
    {
        public string logicalOperator { get; set; }
        public string fieldName { get; set; }
        public string @operator { get; set; }
        public string value { get; set; }
        public int index { get; set; }
    }

    public class Column
    {
        public string name { get; set; }
        public string text { get; set; }
        public int fieldId { get; set; }
        public bool canSortBy { get; set; }
        public int width { get; set; }
        public bool isIdentity { get; set; }
        public int fieldType { get; set; }
    }

    public class Data
    {
        [JsonProperty("ms.vss-work-web.work-item-query-data-provider")]
        public MsVssWorkWebWorkItemQueryDataProvider msvssworkwebworkitemquerydataprovider { get; set; }
        public bool queryRan { get; set; }
        public string wiql { get; set; }
        public bool isLinkQuery { get; set; }
        public bool isTreeQuery { get; set; }
        public List<Column> columns { get; set; }
        public List<SortColumn> sortColumns { get; set; }
        public List<int> sourceIds { get; set; }
        public List<int> linkIds { get; set; }
        public List<int> targetIds { get; set; }
        public List<string> pageColumns { get; set; }
        public Payload payload { get; set; }
        public EditInfo editInfo { get; set; }
    }

    public class EditInfo
    {
        public SourceFilter sourceFilter { get; set; }
        public string treeLinkTypes { get; set; }
        public TreeTargetFilter treeTargetFilter { get; set; }
        public int mode { get; set; }
        public string teamProject { get; set; }
    }

    public class MsVssWorkWebWorkItemQueryDataProvider
    {
        public object errorMessage { get; set; }
        public Data data { get; set; }
        public object versionStamp { get; set; }
    }

    public class Payload
    {
        public List<string> columns { get; set; }
        public List<List<object>> rows { get; set; }
    }

    public class ResolvedProvider
    {
        public string id { get; set; }
    }

    public class Root
    {
        public Data data { get; set; }
        public SharedData sharedData { get; set; }
        public List<ResolvedProvider> resolvedProviders { get; set; }
        public object exceptions { get; set; }
        public object clientProviders { get; set; }
        public object scopeName { get; set; }
        public object scopeValue { get; set; }
    }

    public class SharedData
    {
    }

    public class SortColumn
    {
        public string name { get; set; }
    }

    public class SourceFilter
    {
        public List<Clause> clauses { get; set; }
        public List<object> groups { get; set; }
        public int maxGroupLevel { get; set; }
    }

    public class TreeTargetFilter
    {
        public List<object> clauses { get; set; }
        public List<object> groups { get; set; }
        public int maxGroupLevel { get; set; }
    }
}
