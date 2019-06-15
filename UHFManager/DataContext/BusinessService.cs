using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UHFManager.DataContext
{

    public abstract class BusinessService
    {
        /// <summary>
        /// 执行全局数据相关操作
        /// </summary>
        /// <param name="action"></param>
        public void UsingGlobalDb(Action<GlobalDbContext> action)
        {
            using (var context = new GlobalDbContext())
            {
                action(context);
            }
        }


    }
}
