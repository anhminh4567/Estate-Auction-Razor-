﻿using Microsoft.AspNetCore.SignalR;
using Repository.Database.Model;
using Repository.Interfaces.DbTransaction;
using Service.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.MyHub.HubServices
{
    public class NotificationHubService
    {
        private readonly IHubContext<NotificationHub> _notificationHub;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ConnectionMapping _connectionMapping;
        public NotificationHubService(IHubContext<NotificationHub> notificationHub, IUnitOfWork unitOfWork, ConnectionMapping connectionMapping)
        {
            _notificationHub = notificationHub;
            _unitOfWork = unitOfWork;
            _connectionMapping = connectionMapping;
        }

        public async Task SendNewNotification(string userEmail, Notification notification)
        {
            var connectionId = _connectionMapping.GetConnections(userEmail);
            await _notificationHub.Clients.User(connectionId).SendAsync("OnNewNotification", notification);
        }
    }
}
