using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace UHFManager.DTO
{
    public abstract class RequestBase
    {
        public virtual string cmd { get; set; }

        public virtual int version { get; set; } = 1;
        
        public virtual string GetDataString()
        { 
            PropertyInfo propertyInfo = this.GetType().GetProperty("data");
            if (propertyInfo == null)
            {
                return string.Empty;
            }
            object data = propertyInfo.GetValue(this,null);
            return SimpleJson.SerializeObject(data);
        }
        
    }
}
