using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.WebForms
{
    public class DemoModel
    {
        public DemoModel()
        { }
        public DemoModel(string id, string name)
        {
            this.id = id;
            this.name = name;
        }
        private string id;//编码   

        public string Id
        {
            get { return id; }
            set { id = value; }
        }
        private string name;//名称   

        public string Name
        {
            get { return name; }
            set { name = value; }
        }
    }
}