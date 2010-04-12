using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using MvcExtensions.Services;
using MvcExtensions.UI.ViewModel;
using System.Web.Routing;
using NHibernate.Criterion;
using NHibernate.Linq;

namespace MvcExtensions.UI.Web.Helpers
{
    public class jqGridMetaData
    {
        public class JsonReader
        {
            public string root = "rows";
            public string page = "page";
            public string total = "total";
            public string records = "records";
            public bool repeatitems = false;
            public string cell = null;
            public string id = "Id";
        }

        public class Colspec
        {
            public class Editrules
            {
                // if true, fields hidden in the grid are included as editable in form editing when add or edit methods are called.
                public bool edithidden = false;
                // if true, fields hidden in the grid are included in the search form.
                public bool searchhidden = false;
                // if true the value will be checked and, if it is empty, an error message will be displayed.
                public bool required = false;
                // if true the value will be checked to be sure it is a number and, if it is not, an error message will be displayed.
                public bool number = false;
                // if true the value will be checked to be sure it is an integer and, if it is not, an error message will be displayed.
                public bool integer = false;
                // if set to true, the format of the field is governed by the setting of the datefmt parameter 
                public bool date = false;
            }

            public class EditOptions
            {
                public string value;
            }

            public enum EditType
            {
                text, textarea, select, checkbox, password
            }

            public string name;
            public string index;
            public int width;
            public string align;
            public Editrules editrules = new Editrules();
            public bool editable = true;
            public string edittype = EditType.text.ToString();
            public bool resizable = true;
            public bool search = true;
            public bool sortable = true;
            public EditOptions editoptions = new EditOptions();
            public string formatter;
            public void Dropdown<T>(IEnumerable<T> data, Func<T, string> GetId, Func<T, string> GetDescription)
            {
                Dropdown<T>(data, GetId, GetDescription, null);
            }

            public void Dropdown<T>(IEnumerable<T> data, Func<T,string> GetId,Func<T,string>GetDescription,string NullValue)
            {
                var sb = new StringBuilder();
                bool first = true;
                if (NullValue != null)
                {
                    first = false;
                    sb.Append("0:");
                    sb.Append(NullValue);
                }
                foreach (var v in data)
                {
                    if (first)
                        first = false;
                    else
                        sb.Append(";");

                    sb.Append(GetId(v));
                    sb.Append(":");
                    sb.Append(GetDescription(v).Split('\n')[0].Replace(";","").Replace(":",""));
                }
                editoptions.value = sb.ToString();
                edittype = EditType.select.ToString();
            }
        }

        public string url;
        public string datatype="json";
        public string mtype= "GET";
        public string[] colNames;
        public Colspec[] colModel;
        public string pager;
        public int rowNum = 10;
        public int[] rowList=new int[] {5,10,20,50};
        public bool sortable = true;
        public string sortname = "Id";
        public string sortorder= "desc";
        public bool viewrecords= true;
        public string imgpath;
        public string caption;
        public JsonReader jsonReader = new JsonReader();
        public string editurl;


        public jqGridMetaData SetCol(Func<Colspec,bool> selector,Action<Colspec> c)
        {
            foreach(var v in colModel.Where(x=>selector(x)))
                c(v);
            return this;
        }
    }

    public class IMjqGridPostBack
    {
        public int page {get;set;}
        public int rows {get;set;}
        public string sidx {get;set;}
        public string sort { get; set; }
        public bool _search { get; set; }
        public string searchField { get; set; }
        public string searchOper { get; set; }
        public string searchString { get; set; }


        public IMjqGridPostBack()
        {
            page = 1;
            rows = 10;
            sort = "asc";
            _search = false;
        }
    }

    public static class jqGridHelper
    {
        public static jqGridMetaData GetMetaData<T>(string Caption, string dataurl, string editurl,params string[] IgnoreFields) 
        {
            var x = new jqGridMetaData() { 
                caption = Caption,
                url = dataurl,
                editurl = editurl
            };
            var l = new List<jqGridMetaData.Colspec>();
            var numbertypes = (new Type[] { typeof(int), typeof(decimal), typeof(double) });
            var inttypes = (new Type[] { typeof(int)});
            foreach (var v in typeof(T).GetProperties())
            {
                if (IgnoreFields.Contains(v.Name)) 
                    continue;
                var c = new jqGridMetaData.Colspec()
                {
                    name = v.Name,
                    index = v.Name,
                    editable = v.Name.ToLower()!="id",
                    align = numbertypes.Contains(v.PropertyType) ? "right" : "left",
                    editrules = new jqGridMetaData.Colspec.Editrules()
                    {
                        number = numbertypes.Contains(v.PropertyType),
                        integer = inttypes.Contains(v.PropertyType),
                        date = typeof(DateTime) == v.PropertyType || typeof(DateTime?) == v.PropertyType 
                    }
                    ,edittype = jqGridMetaData.Colspec.EditType.text.ToString()
                };
                if (v.PropertyType == typeof(bool))
                {
                    c.edittype = jqGridMetaData.Colspec.EditType.checkbox.ToString();
                    c.editoptions.value = "true:false";
                    c.formatter = "checkbox";
                }
                if (c.editrules.date)
                {
                    c.formatter = "date";
                }
                l.Add(c);
            }
            x.colModel = l.ToArray();
            x.colNames = x.colModel.Select(z => z.name).ToArray();
            return x;
        }

        public static IQueryable<T> FilterCriteria<T>(INHibernateRepository sRepo, IMjqGridPostBack pb) where T : class 
        {
            return sRepo.Find<T>(s => {
                if (pb._search)
                {
                    switch (pb.searchOper)
                    {
                        //['bw','eq','ne','lt','le','gt','ge','ew','cn'] 
                        case "bw": s.Add(Expression.Like(pb.searchField, pb.searchString+"%")); break;
                        case "eq": s.Add(Expression.Eq(pb.searchField, pb.searchString)); break;
                        case "ne": s.Add(Expression.Not(Expression.Eq(pb.searchField, pb.searchString))); break;
                        case "lt": s.Add(Expression.Lt(pb.searchField, pb.searchString)); break;
                        case "le": s.Add(Expression.Le(pb.searchField, pb.searchString)); break;
                        case "gt": s.Add(Expression.Gt(pb.searchField, pb.searchString)); break;
                        case "ge": s.Add(Expression.Ge(pb.searchField, pb.searchString)); break;
                        case "ew": s.Add(Expression.Eq(pb.searchField, "%" + pb.searchString)); break;
                        case "cn": s.Add(Expression.Like(pb.searchField, "%" + pb.searchString + "%")); break;
                    }
                }
                if (!string.IsNullOrEmpty(pb.sidx))
                {
                    if (pb.sort == "asc")
                        s.AddOrder(Order.Asc(pb.sidx));
                    else
                        s.AddOrder(Order.Desc(pb.sidx));
                }
            });
        }

        public static object GetData<T>(IQueryable<T> data, IMjqGridPostBack pb,Action<T> ModifyBeforeConversion) where T:class
        {
            var c = data.Count();
            var d =data.Skip((pb.page - 1) * pb.rows).Take(pb.rows).ToArray();
            foreach (var it in d)
                ModifyBeforeConversion(it);
            
            return new
            {
                rows = d,
                page = pb.page,
                total = (int)(Math.Ceiling((double)c / pb.rows) ),
                records = c
            };
        }
    }
}
