using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;

namespace GlobalizationExample
{
    public class LocalizedDescriptionAttribute : DescriptionAttribute
    {
        static readonly Dictionary<Type, ResourceManager> s_resourceManagers;
        string _resourceKey;
        ResourceManager _resourceManager;
        string _description;
        static LocalizedDescriptionAttribute()
        {
            s_resourceManagers = new Dictionary<Type, ResourceManager>();
        }

        public LocalizedDescriptionAttribute(string resourceKey, Type resourceType)
        {
            _resourceKey = resourceKey;
            if (!s_resourceManagers.TryGetValue(resourceType, out _resourceManager))
            {
                _resourceManager = new ResourceManager(resourceType);
                s_resourceManagers[resourceType] = _resourceManager;
            }
        }

        public override string Description
        {
            get
            {
                if (false == string.IsNullOrWhiteSpace(_description)) { return _description; }
                _description = _resourceManager.GetString(_resourceKey);
                if(string.IsNullOrWhiteSpace(_description))
                {
                    _description = $"[#Error: {_resourceKey}]";
                }
                return _description;
            }
        }
    }
}
