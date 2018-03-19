using DJS.Core.CPlatform.Transport;
using System.Collections.Generic;

namespace DJS.Core.CPlatform.Server
{

    public interface IServerHostProvider
    {
        /// <summary>
        /// 设置调度器订阅客户端
        /// </summary>
        /// <param name="messageSender"></param>
        void SetSubSchedulerClient(IMessageSender messageSender);
        /// <summary>
        /// 获取所有调度器订阅客户端
        /// </summary>
        /// <returns></returns>
        List<IMessageSender> GetSubSchedulerClients();
        /// <summary>
        /// 移除指定调度器订阅客户端
        /// </summary>
        /// <param name="clientId"></param>
        void RemoveSubSchedulerClient(string clientId);
        /// <summary>
        /// 发布调度器任务
        /// </summary>
        void PublishSchedulerJobs();

        /// <summary>
        /// 设置执行器订阅客户端
        /// </summary>
        /// <param name="messageSender"></param>
        void SetSubExecuteClient(IMessageSender messageSender);
        /// <summary>
        /// 获取所有执行器订阅客户端
        /// </summary>
        /// <returns></returns>
        List<IMessageSender> GetSubExecuteClients();
        /// <summary>
        /// 移除指定执行器订阅客户端
        /// </summary>
        /// <param name="clientId"></param>
        void RemoveSubExecuteClient(string clientId);
        /// <summary>
        /// 发布执行器任务
        /// </summary>
        void PublishExecuteJobs(string jobId);

        /// <summary>
        /// 发布执行器任务信息
        /// </summary>
        void PublishExecuteJobFiles(string jobId);
    }
}
