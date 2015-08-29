using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
namespace CacheDB.Net
{
    public class Factory
    {
               
        public static ICachedManager GetCachedManager(string section)
        {
            CacheDbSection cds = (CacheDbSection)System.Configuration.ConfigurationManager.GetSection(section);
            if (cds == null)
                throw new CacheDBException(section + " section not found!");
            return GetCachedManager(cds);
        }

        public static ICachedManager GetCachedManager()
        {
            return GetCachedManager(CacheDbSection.Instance);
        }

        private static ICachedManager GetCachedManager(CacheDbSection section)
        {
            implement.CachedManager cm = new implement.CachedManager();
            DbManagerConfig dbconfig = section.DbManager;
            cm.DBManager = CreateDBManager(dbconfig);
            foreach (LevelCachedConfig config in section.Levels)
            {
                cm.AddLevel(config.Name, config.Maximum, config.UpgradeValue);
            }
            foreach (PolicyConfig policy in section.Policies)
            {
                CreatePolicy(policy, cm);
            }
            cm.DBManager.Open();
            return cm;
        }

        public static IDBManager CreateDBManager(DbManagerConfig config)
        {
            IDBManager result = null;
            Type dbtype = Type.GetType(config.Type);
            if (dbtype == null)
                throw new CacheDBException("{0} dbmanager type notfound!", config.Type);
            Type formaterType = Type.GetType(config.Formater);
            if (formaterType == null)
                throw new CacheDBException("{0} formater notfound!", config.Formater);
            result = (IDBManager)Activator.CreateInstance(dbtype);
            result.Formater = (IFormater)Activator.CreateInstance(formaterType);
            foreach (PropertyConfig p in config.Properties)
            {
                PropertyInfo pi = dbtype.GetProperty(p.Name, BindingFlags.Public | BindingFlags.Instance);
                if (pi != null && pi.CanWrite)
                {
                    try
                    {
                        pi.SetValue(result, Convert.ChangeType(p.Value, pi.PropertyType), null);
                    }
                    catch
                    {
                    }
                }
            }
            return result;
            
        }
        
        private static void CreatePolicy(PolicyConfig config,ICachedManager cm)
        {
            IPolicy result = null;
            Type objectType = Type.GetType(config.ObjectType);
            Type policyType = Type.GetType(config.Type);
            if(objectType ==null)
                throw new CacheDBException("{0} object type notfound!", config.Type);
            if(policyType==null)
                throw new CacheDBException("{0} Policy type notfound!", config.Type);
            result = (IPolicy)Activator.CreateInstance(policyType);

            foreach (PropertyConfig p in config.Properties)
            {
                PropertyInfo pi = policyType.GetProperty(p.Name, BindingFlags.Public | BindingFlags.Instance);
                if (pi != null && pi.CanWrite)
                {
                    try
                    {
                        pi.SetValue(result, Convert.ChangeType(p.Value, pi.PropertyType), null);
                    }
                    catch
                    {
                    }
                }
            }
            cm.AddPolicy(objectType, result);
          
        }

       
    }
}
