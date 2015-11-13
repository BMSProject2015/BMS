using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DomainModel.Abstract;
using System.Collections.Specialized;
using DomainModel;
using System.Data;
using DomainModel.Controls;
using System.Data.SqlClient;
using System.Web.Mvc;
namespace VIETTEL.Models
{
    public class DanhMucNganhModels
    {
        public static DataTable LayDanhSachNganhCon()
        {
            String SQL = String.Format(@"SELECT DISTINCT sNG
                                        FROM NS_MucLucNganSach
                                        WHERE iTrangThai=1 AND sNG<>''
                                        ORDER BY sNG");
            DataTable dt = Connection.GetDataTable(SQL);
            return dt;
        }
        public static DataTable LayDanhSachNganh()
        {
            String SQL = String.Format(@"SELECT iID_MaNganh,iID_MaNganhMLNS
                                        FROM NS_MucLucNganSach_Nganh ");
            DataTable dt = Connection.GetDataTable(SQL);
            return dt;
        }
    }
}