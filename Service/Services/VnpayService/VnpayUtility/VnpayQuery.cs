﻿using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Protocols;
using Repository.Database.Model;
using Service.Services.VnpayService.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;


namespace Service.Services.VnpayService.VnpayUtility
{
    public class VnpayQuery
    {
        public VnpayQuery()
        {
        }
        public VnpayQueryResult? btnQuery_Click(HttpContext httpContext, Transaction transaction)
        {

            var vnp_Api = VnpayDefaultValue.Vnp_Api;
            var vnp_HashSecret = VnpayDefaultValue.Vnp_HashSecret; //Secret KEy
            var vnp_TmnCode = VnpayDefaultValue.Vnp_TmnCode; // Terminal Id

            var vnp_RequestId = DateTime.Now.Ticks.ToString(); //Mã hệ thống merchant tự sinh ứng với mỗi yêu cầu truy vấn giao dịch. Mã này là duy nhất dùng để phân biệt các yêu cầu truy vấn giao dịch. Không được trùng lặp trong ngày.
            var vnp_Version = VnPayLibrary.VERSION; //2.1.0
            var vnp_Command = "querydr";
            var vnp_TxnRef = transaction.vnp_TxnRef; //orderId.Text; // Mã giao dịch thanh toán tham chiếu
            var vnp_OrderInfo = "Truy van giao dich:" + transaction.vnp_TxnRef;//orderId.Text;
            var vnp_TransactionDate = transaction.vnp_TransactionDate;//payDate.Text;
            var vnp_CreateDate = DateTime.Now.ToString("yyyyMMddHHmmss");
            var vnp_IpAddr = Utils.GetIpAddress(httpContext);

            var signData = vnp_RequestId + "|" + vnp_Version + "|" + vnp_Command + "|" + vnp_TmnCode + "|" + vnp_TxnRef + "|" + vnp_TransactionDate + "|" + vnp_CreateDate + "|" + vnp_IpAddr + "|" + vnp_OrderInfo;
            var vnp_SecureHash = Utils.HmacSHA512(vnp_HashSecret, signData);

            var qdrData = new
            {
                vnp_RequestId,
                vnp_Version,
                vnp_Command,
                vnp_TmnCode,
                vnp_TxnRef,
                vnp_OrderInfo,
                vnp_TransactionDate,
                vnp_CreateDate,
                vnp_IpAddr,
                vnp_SecureHash

            };
            var jsonData = JsonSerializer.Serialize(qdrData);//.Serialize(qdrData);

            using (var vnpayClient = new HttpClient() { BaseAddress = new Uri(vnp_Api), })
            {
                //vnpayClient.DefaultRequestHeaders.Add("Content-Type", "application/json");
                HttpContent content = new StringContent(jsonData, Encoding.UTF8, "application/json");//(jsonData);
                var response = vnpayClient.PostAsync(new Uri(vnp_Api), content).Result;
                var contentBody = response.Content.ReadFromJsonAsync<VnpayQueryResult>().Result;
                return contentBody;
            }
            //var httpWebRequest = (HttpWebRequest)WebRequest.Create(vnp_Api);
            //httpWebRequest.ContentType = "application/json";
            //httpWebRequest.Method = "POST";

            //using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            //{
            //	streamWriter.Write(jsonData);
            //}
            //var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            //var strData = "";
            //using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            //{
            //	strData = streamReader.ReadToEnd();
            //}
            //display.InnerHtml = "<b>VNPAY RESPONSE:</b> " + strData;
        }
    }
}
