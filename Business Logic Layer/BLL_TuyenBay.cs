﻿using Data_Access_Layer;
using Data_Transfer_Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_Logic_Layer
{
    public class BLL_TuyenBay
    {
        public static List<TuyenBay> GetTuyenBays()
        {
            var tuyenBays = DAL_TuyenBay.GetTuyenBays();
            if (tuyenBays == null) return new List<TuyenBay>();
            return tuyenBays;
        }
        public static TuyenBay GetTuyenBay(int maTB)
        {
            return DAL_TuyenBay.GetTuyenBay(maTB);
        }
        public static List<TuyenBay> searchMaTB (string maTB)
        {
            var tuyenBays = DAL_TuyenBay.SearchMaTB(maTB);
            if (tuyenBays == null) return new List<TuyenBay>();
            return tuyenBays;
        }
        public static List<TuyenBay> searchTenSBDi (string tenSB)
        {
            var tuyenBays = DAL_TuyenBay.SearchTenSBDi(tenSB);
            if (tuyenBays == null) return new List<TuyenBay>();
            return tuyenBays;
        }
        public static List<TuyenBay> searchTenSBDen (string tenSB)
        {
            var tuyenBays = DAL_TuyenBay.SearchTenSBDen(tenSB);
            if (tuyenBays == null) return new List<TuyenBay>();
            return tuyenBays;
        }
    }
}
