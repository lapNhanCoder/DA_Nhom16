using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.IO;
namespace RandomPassword
{
    internal class Rating
    {
        private const string chuThuong = "abcdefghijklmnopqrstuvwxyz";
        private const string chuHoa = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        private const string so = "0123456789";
        private const string kyTu = "!@#$%^&*()-_=+[{]};:<>|./?";
        //Hàm kiểm tra mật khẩu có rò rỉ hay không 
        public bool KiemTraRoRi(string matKhau)
        {
            string filePath = "leaked.txt";
            if (!File.Exists(filePath))
            {
                return false;
            }
            var leakedPasswords = File.ReadAllLines(filePath);
            // Sử dụng String.Equals() để so sánh matKhau với từng mật khẩu rò rỉ
            return leakedPasswords.Any(p => string.Equals(matKhau, p, StringComparison.OrdinalIgnoreCase));
        }
        public int RatingPassword(string matKhau)
        {
            int score = 0; //biến điểm để đánh giá độ mạnh mật khẩu
            if (matKhau.Length == 0)
                return score;
            //1.Đánh giá điểm dựa trên độ dài, tối đa 50 điểm
            //Nếu mật khẩu có 13 ký tự trở lên thì tự động điểm = 50
            if (matKhau.Length >= 13)
                score = 50;
            else
                //Mỗi ký tự 4 điểm
                score = matKhau.Length * 4;

            //2.Đánh giá điểm dựa trên sự đa dạng ký tự
            bool coThuong = matKhau.Any(c => chuThuong.Contains(c)); //biến kiểm tra mật khẩu có ký tự thường hay không
            bool coHoa = matKhau.Any(c => chuHoa.Contains(c)); //biến kiểm tra mật khẩu có ký tự hoa hay không
            bool coSo = matKhau.Any(c => so.Contains(c)); //biến kiểm tra mật khẩu có ký tự số hay không
            bool coKiTu = matKhau.Any(c => kyTu.Contains(c)); //biến kiểm tra mật khẩu có ký tự đặc biệt hay không

            //Đếm số loại ký tự có sử dụng
            int demLoai = 0;
            if (coThuong)
            {
                score += 10;
                demLoai++;
            }
            if (coHoa)
            {
                score += 10;
                demLoai++;
            }
            if (coSo)
            {
                score += 10;
                demLoai++;
            }
            if (coKiTu)
            {
                score += 10;
                demLoai++;
            }

            //3.Phạt nếu chỉ dụng một loại ký tự
            if (demLoai == 1)
            {
                score -= 30;
            }

            //4.Phạt nếu nằm trong danh sách các mật khẩu phổ biến
            if (KiemTraRoRi(matKhau))
            {
                score = 0;
            }

            // Đảm bảo điểm không bị âm 
            if (score < 0)
                score = 0;
            return score;
        }
    }
}
