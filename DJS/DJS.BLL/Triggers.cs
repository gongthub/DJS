using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DJS.BLL
{
   public class Triggers
    { 
        #region 属性
        /// <summary>
        /// 触发器接口
        /// </summary>
        private static IDAL.ITriggers iTriggers = null;

        #endregion
        
        #region 构造函数

        static Triggers()
        {
            iTriggers = DJS.DAL.DataAccessFactory.DataAccessFactory.CreateITriggers();
        }

        #endregion
        
        #region  获取所有Quartz中 Triggers +static List<Model.Triggers> GetTriggersForQuartz()
        /// <summary>
        /// 获取所有Quartz中 Triggers
        /// </summary>
        /// <returns></returns>
        public static List<Model.Triggers> GetTriggersForQuartz()
        {
            List<Model.Triggers> models = new List<Model.Triggers>();

            IList<string> groups = Common.QuartzHelp.quartzHelp.GetTriggerGroupNames();
            if (groups != null && groups.Count > 0)
            {
                foreach (string group in groups)
                {
                    ISet<TriggerKey> triggers = Common.QuartzHelp.quartzHelp.GetTriggerKeys(group);
                    if (triggers != null && triggers.Count > 0)
                    {
                        foreach (TriggerKey trigger in triggers)
                        {
                            ITrigger Trigger = Common.QuartzHelp.quartzHelp.GetTrigger(trigger);
                            if (Trigger != null)
                            {
                                Model.Triggers model = new Model.Triggers();
                                model.Name = trigger.Name;
                                model.GroupName = trigger.Group; 
                                models.Add(model);
                            }
                        }
                    }
                }
            }

            return models;
        }

        /// <summary>
        /// 获取所有Quartz中 Triggers
        /// </summary>
        /// <returns></returns>
        public static List<Model.Triggers> GetTriggersForQuartz(Predicate<Model.Triggers> m)
        {
            List<Model.Triggers> models = GetTriggersForQuartz();
            if (models != null && models.Count > 0)
            {
                models = models.FindAll(m);
            }
            return models;
        }
        #endregion

    }
}
