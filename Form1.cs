using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;

namespace RandomPassword
{
    public partial class Form1 : Form
    {
        private Random random = new Random();
        List<TextBox> danhSachTextBox;
        List<Button> danhSachButton;
        public Form1()
        {
            InitializeComponent();
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            txtDoDai.Text = "";
            cboSoLuong.SelectedIndex = 0;

            foreach (TextBox txt in danhSachTextBox)
            {
                txt.Visible = false;
            }
            
            foreach (Button btn in danhSachButton)
            {
                btn.Visible = false;
            }
            checkThuong.Checked = false;
            checkHoa.Checked = false;
            checkSo.Checked = false;
            checkKyTu.Checked = false;

            txtDoDai.Focus();
        }

        private void btnTaoMatKhau_Click(object sender, EventArgs e)
        {
            RandomPassword rp = new RandomPassword();
            int doDaiMatKhau = 0; //biến chứa độ dài mật khẩu
            if (!int.TryParse(txtDoDai.Text, out doDaiMatKhau) || doDaiMatKhau <= 0)
            {
                MessageBox.Show("Vui lòng nhập độ dài mật khẩu là một số nguyên dương",
                    "Thiếu tùy chọn", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            bool coThuong = checkThuong.Checked;
            bool coHoa = checkHoa.Checked;
            bool coSo = checkSo.Checked;
            bool coKyTu = checkKyTu.Checked;

            // Kiểm tra xem người dùng có chọn ít nhất 1 loại ký tự không
            if (!coThuong && !coHoa && !coSo && !coKyTu)
            {
                MessageBox.Show("Vui lòng chọn ít nhất một loại ký tự (abc, ABC, 123,...)",
                    "Thiếu tùy chọn", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            //Lấy số lượng mật khẩu muốn tạo từ combo box
            if (cboSoLuong.SelectedItem == null)
            {
                cboSoLuong.SelectedItem = 0; //mặc định chọn 1 nếu chưa chọn
            }

            const string FILE_MATKHAU = "passwords.txt"; //file lưu mật khẩu đã tạo
            List<string> matKhauDaTao = new List<string>(); //list chứa các mật khẩu đã tạo
            //Nếu file không tồn tại sẽ trả về một List<string> rỗng
            if (System.IO.File.Exists(FILE_MATKHAU))
            {
                // Đọc tất cả các dòng từ file và thêm vào danh sách mật khẩu đã tạo
                matKhauDaTao.AddRange(System.IO.File.ReadAllLines(FILE_MATKHAU));
            }

            List<string> matKhauLanNay = new List<string>(); 
            //Danh sách tạm để lưu các mật khẩu đã sinh ra trong lần bấm này

            int soLuongCanTao = int.Parse(cboSoLuong.SelectedItem.ToString()); 
            //biến chứa số lượng mật khẩu muốn tạo

            //Vòng lặp để ẩn, hiện và gán mật khẩu được tạo vào các Textbox
            for (int i = 0; i < danhSachTextBox.Count; i++)
            {
                if (i < soLuongCanTao)
                {
                    danhSachTextBox[i].Visible = true;

                    string matKhauMoi = ""; //biến chứa mật khẩu mới được tạo

                    //chạy vòng lặp do-while để kiểm tra mật khẩu đã có trùng không, nếu trùng thì tạo lại
                    do
                    {
                        matKhauMoi = rp.TaoMatKhau(doDaiMatKhau, coThuong, coHoa, coSo, coKyTu);
                    }
                    while (matKhauDaTao.Contains(matKhauMoi)&&matKhauLanNay.Contains(matKhauMoi));
                    //kiểm tra trùng lặp với tất cả các mật khẩu đã tạo trước đó từ file
                    
                    //thêm mật khẩu mới vào danh sách mật khẩu đã tạo từ trước và mật khẩu tạo trong lần này
                    matKhauDaTao.Add(matKhauMoi);
                    matKhauLanNay.Add(matKhauMoi);

                    danhSachTextBox[i].Text = matKhauMoi;

                }
                else
                {
                    danhSachTextBox[i].Visible = false;
                    danhSachTextBox[i].Text = "";
                }
            }

            //Ghi tất cả mật khẩu tạo mới lần này vào cuối file mà không làm mất các mật khẩu đã có
            if (matKhauLanNay.Count > 0)
            {
                try
                {
                    System.IO.File.AppendAllLines(FILE_MATKHAU, matKhauLanNay);

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi không ghi file mật khẩu!!");
                }
            }

            if (cboSoLuong.SelectedItem != null)
            {   
                if(int.TryParse(cboSoLuong.SelectedItem.ToString(), out int soLuong))
                for (int j = 0; j < danhSachButton.Count; j++)
                {   
                    //Vòng lặp ẩn, hiện các button Copy
                    if (j < soLuong)
                    {
                        danhSachButton[j].Visible = true;
                    }
                    else
                    {
                        danhSachButton[j].Visible = false;
                    }
                }
                else
                {
                    MessageBox.Show("Vui lòng chọn một số hợp lệ!");
                }
            }
            else
            {
                for (int i = 0; i < danhSachButton.Count; i++)
                {
                    danhSachButton[i].Visible = false;
                }
            }
        }
        private void btnRating_Click(object sender, EventArgs e)
        {
            Rating rating = new Rating();
            int score = rating.RatingPassword(txtRandomPassword1.Text); //biến điểm để đánh giá độ mạnh mật khẩu
            if (score < 30)
                MessageBox.Show("Mật khẩu yếu!", "Đánh giá", MessageBoxButtons.OK, MessageBoxIcon.Information);
            else if (score < 50)
                MessageBox.Show("Mật khẩu trung bình!", "Đánh giá", MessageBoxButtons.OK, MessageBoxIcon.Information);
            else if (score < 75)
                MessageBox.Show("Mật khẩu khá!", "Đánh giá", MessageBoxButtons.OK, MessageBoxIcon.Information);
            else
                MessageBox.Show("Mật khẩu mạnh!", "Đánh giá", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnManh_Click(object sender, EventArgs e)
        {
            try
            {
                //Gọi hàm reset 
                btnReset_Click(sender, e);

                int doDai = random.Next(12, 15); //biến chứa độ dài mật khẩu
                txtDoDai.Text = doDai.ToString();
                checkThuong.Checked = true;
                checkHoa.Checked = true;
                checkSo.Checked = true;
                checkKyTu.Checked = true;

            }
            catch (Exception)
            {
                MessageBox.Show("Lỗi!");
            }
        }

        private void btnKha_Click(object sender, EventArgs e)
        {
            try
            {
                //Gọi hàm reset 
                btnReset_Click(sender, e);

                int doDai = random.Next(10, 12); //biến chứa độ dài mật khẩu
                txtDoDai.Text = doDai.ToString();
                checkThuong.Checked = true;
                checkHoa.Checked = true;
                checkSo.Checked = true;

            }
            catch (Exception)
            {
                MessageBox.Show("Lỗi!");
            }
        }

        private void btnTrungBinh_Click(object sender, EventArgs e)
        {
            try
            {
                //Gọi hàm reset 
                btnReset_Click(sender, e);

                int doDai = random.Next(5, 10); //biến chứa độ dài mật khẩu
                txtDoDai.Text = doDai.ToString();
                checkThuong.Checked = true;
                checkHoa.Checked = true;

            }
            catch (Exception)
            {
                MessageBox.Show("Lỗi!");
            }
        }

        private void btnYeu_Click(object sender, EventArgs e)
        {
            try
            {
                //Gọi hàm reset 
                btnReset_Click(sender, e);

                int doDai = random.Next(0, 5); //biến chứ độ dài mật khẩu
                txtDoDai.Text = doDai.ToString();
                checkThuong.Checked = true;
            }
            catch (Exception)
            {
                MessageBox.Show("Lỗi!");
            }
        }
        private void copy(TextBox txt)
        {
            if (!string.IsNullOrEmpty(txt.Text))
            {
                Clipboard.SetText(txt.Text);//SAo chép nội dung vào clipboard
                MessageBox.Show("Đã copy mật khẩu!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Không có mật khẩu để copy!", "Cảnh báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            danhSachTextBox = new List<TextBox>() {
            txtRandomPassword1, txtRandomPassword2, txtRandomPassword3,
            txtRandomPassword4, txtRandomPassword5, txtRandomPassword6
            };

            danhSachButton = new List<Button>() {
            btnCopy1, btnCopy2, btnCopy3,
            btnCopy4, btnCopy5, btnCopy6
            };

            foreach (TextBox txt in danhSachTextBox)
            {
                txt.Visible = false;
            }

            foreach (Button btn in danhSachButton)
            {
                btn.Visible = false;
            }

            if (cboSoLuong.Items.Count > 0)
            {
                cboSoLuong.SelectedIndex = 0;
                //set mặc định cho combobox là giá trị đầu khi mở form1
            }
        }

        private void btnCopy1_Click(object sender, EventArgs e)
        {
            copy(txtRandomPassword1);
        }

        private void btnCopy2_Click(object sender, EventArgs e)
        {
            copy(txtRandomPassword2);
        }

        private void btnCopy3_Click(object sender, EventArgs e)
        {
            copy(txtRandomPassword3);
        }

        private void btnCopy4_Click(object sender, EventArgs e)
        {
            copy(txtRandomPassword4);
        }

        private void btnCopy5_Click(object sender, EventArgs e)
        {
            copy(txtRandomPassword5);
        }

        private void btnCopy6_Click(object sender, EventArgs e)
        {
            copy(txtRandomPassword6);
        }
    }
    }

