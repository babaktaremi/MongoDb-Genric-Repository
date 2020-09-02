using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace MongoDbTest.Extensions
{
    public class TypeDescriptorOverridingProvider : TypeDescriptionProvider
    {
        private readonly ICustomTypeDescriptor ctd;

        public TypeDescriptorOverridingProvider(ICustomTypeDescriptor ctd)
        {
            this.ctd = ctd;
        }

        public override ICustomTypeDescriptor GetTypeDescriptor(Type objectType, object instance)
        {
            return ctd;
        }
    }

    public class PropertyOverridingTypeDescriptor : CustomTypeDescriptor
    {
        private readonly Dictionary<string, PropertyDescriptor> overridePds = new Dictionary<string, PropertyDescriptor>();

        public PropertyOverridingTypeDescriptor(ICustomTypeDescriptor parent)
            : base(parent)
        { }

        public void OverrideProperty(PropertyDescriptor pd)
        {
            overridePds[pd.Name] = pd;
        }

        public override object GetPropertyOwner(PropertyDescriptor pd)
        {
            object o = base.GetPropertyOwner(pd);

            if (o == null)
            {
                return this;
            }

            return o;
        }

        public PropertyDescriptorCollection GetPropertiesImpl(PropertyDescriptorCollection pdc)
        {
            List<PropertyDescriptor> pdl = new List<PropertyDescriptor>(pdc.Count + 1);

            foreach (PropertyDescriptor pd in pdc)
            {
                if (overridePds.ContainsKey(pd.Name))
                {
                    pdl.Add(overridePds[pd.Name]);
                }
                else
                {
                    pdl.Add(pd);
                }
            }

            PropertyDescriptorCollection ret = new PropertyDescriptorCollection(pdl.ToArray());

            return ret;
        }

        public override PropertyDescriptorCollection GetProperties()
        {
            return GetPropertiesImpl(base.GetProperties());
        }
        public override PropertyDescriptorCollection GetProperties(Attribute[] attributes)
        {
            return GetPropertiesImpl(base.GetProperties(attributes));
        }
    }
}
