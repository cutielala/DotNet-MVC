﻿using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.DataAcess;
using BulkyBook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulkyBook.DataAccess.Repository
{
    public class OrderHeaderRepository : Repository<OrderHeader>, IOrderHeaderRepository
    {
        private readonly ApplicationDBContext _db;

        public OrderHeaderRepository(ApplicationDBContext db):base(db)
        {
            _db = db;
        }

        public void Update(OrderHeader obj)
        {
            _db.OrderHeaders.Update(obj);
        }
        public void UpdateStatus(int id, string orderStatus, string? paymentStatus)
        {
            var orderFromDb = _db.OrderHeaders.FirstOrDefault(u => u.Id == id);
            if (orderFromDb != null)
            {
                orderFromDb.OrderStatus = orderStatus;
                if (paymentStatus != null)
                {
                    orderFromDb.PaymentStatus = paymentStatus;
                }

            }


        }
        public void UpdateStripePaymentId(int id, string sessionId, string paymentItemId)
        {
            var orderFromDb = _db.OrderHeaders.FirstOrDefault(u => u.Id == id);
            orderFromDb.PaymentDate = DateTime.Now; 
            orderFromDb.SessionId = sessionId;  
            orderFromDb.PaymentIntenId = paymentItemId;

        }
    }
}
