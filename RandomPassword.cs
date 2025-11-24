using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandomPassword
{
    internal class RandomPassword
    {
        private static Random random = new Random(); //hàm random để sinh số ngẫu nhiên
        private const string chuThuong = "abcdefghijklmnopqrstuvwxyz"; //hằng số chứa các ký tự thường
        private const string chuHoa = "ABCDEFGHIJKLMNOPQRSTUVWXYZ"; //hằng số chứa các ký tự hoa
        private const string so = "0123456789"; //hằng số chứa các ký tự số
        private const string kyTu = "!@#$%^&*()-_=+[{]};:<>|./?"; //hằng số chứa các ký tự đặc biệt
                                                                  //Hàm kiểm tra sự rò rỉ của mật khẩu
       
        //Hàm tạo mật khẩu ngẫu nhiên 
        public string TaoMatKhau(int doDai, bool coThuong, bool coHoa, bool coSo, bool coKyTu)
        {
            string tapKyTu = ""; 
            if (coThuong) 
                tapKyTu += chuThuong;
            if (coHoa) 
                tapKyTu += chuHoa;
            if (coSo) 
                tapKyTu += so;
            if (coKyTu) 
                tapKyTu += kyTu;

            StringBuilder matKhau = new StringBuilder(); //chuỗi chứa mật khẩu ngẫu nhiên được sinh ra
            for (int i = 0; i < doDai; i++)
            {
                int index = random.Next(0, tapKyTu.Length);
                //biến index dùng để chọn ngẫu nhiên một ký tự trong tapkytu đã xây dựng ở trên
                matKhau.Append(tapKyTu[index]);
            }
            return matKhau.ToString();
        }
    }
}

