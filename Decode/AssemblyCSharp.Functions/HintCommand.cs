using System.Collections.Generic;
using System.Linq;

namespace AssemblyCSharp.Functions;

internal class HintCommand
{
	internal int selectedIndex;

	internal bool isShow;

	internal int scrollValue;

	internal int width;

	internal int height;

	internal int x;

	internal int y;

	private string chatBack = string.Empty;

	private List<string[]> hints;

	private List<string[]> hints_default = new List<string[]>
	{
		new string[2] { "/tbb", "Hiển thị thông tin Boss xuất hiện" },
		new string[2] { "/hsX", "Auto sáo đè khi id X dưới 50% hp, hoặc chĩa vào NV chat /hs" },
		new string[2] { "/kssp", "KS SUPER BROLY" },
		new string[2] { "/dmt", "Auto chuyển mục tiêu - Dùng để up bí kiếp" },
		new string[2] { "/abt", "Auto dùng bông tai khi HP dưới 50%" },
		new string[2] { "/cd", "Auto cho đậu" },
		new string[2] { "/td", "Auto thu đậu và cất đậu vào rương đồ" },
		new string[2] { "/xd", "Auto xin đậu" },
		new string[2] { "/axd", "Auto spam xin đậu thoát ra vào lại liên tục" },
		new string[2] { "/akok", "Auto jump - Hỗ trợ up đệ kaioken - Chống mất kết nối game" },
		new string[2] { "/dtnsq", "Up đệ tử né siêu quái - Đệ đi theo sư phụ và đánh quái" },
		new string[2] { "/aflag", "Auto bật cờ xám" },
		new string[2] { "/petgohome", "Auto cho đệ về nhà khi tách hợp thể" },
		new string[2] { "/ttdt", "Hiển thị thông tin - chỉ số đệ tử" },
		new string[2] { "/addc", "Thêm nhân vật đang trỏ vào d/s trị thương đặc biệt" },
		new string[2] { "/arc", "Auto trị thương theo d/s trị thương đặc biệt" },
		new string[2] { "/ahs", "Auto dùng ngọc để hồi sinh" },
		new string[2] { "/phpX", "Auto dùng đậu thần khi đệ tử dưới X% HP" },
		new string[2] { "/hpX", "Auto cộng chỉ số lên X HPG" },
		new string[2] { "/hpX", "Auto cộng chỉ số lên X HPG" },
		new string[2] { "/kiX", "Auto cộng chỉ số lên X KIG" },
		new string[2] { "/sdX", "Auto cộng chỉ số lên X SĐG" },
		new string[2] { "/akX", "Auto dùng skill sau X mili giây [1 giây = 1000ms], X=0 => Tắt" },
		new string[2] { "/hskill", "Auto di chuyển đến DHVT23 để hồi skill" },
		new string[2] { "/kzX", "Auto chen vào khu X khi khu có dư slot" },
		new string[2] { "/lcm", "Khóa không cho nhân vật chuyển map" },
		new string[2] { "/lcz", "Khóa không cho nhân vật chuyển khu" },
		new string[2] { "/cX", "Chỉnh tốc độ chạy thành X [Mặc định 7]" },
		new string[2] { "/sX", "Chỉnh tốc độ game thành X [Mặc định 2]" },
		new string[2] { "/rX", "Dịch phải X đơn vị" },
		new string[2] { "/lX", "Dịch trái X đơn vị" },
		new string[2] { "/uX", "Dịch trên X đơn vị" },
		new string[2] { "/dX", "Dịch phải X đơn vị" },
		new string[2] { "/frzX", "Fake time hồi chiêu thành X mili giây, X = 0 ==> Đóng băng skill" },
		new string[2] { "/spos", "Lưu vị trí khi Goback" },
		new string[2] { "/szone", "Lưu khu khi Goback" },
		new string[2] { "/smap", "Lưu map khi Goback" },
		new string[2] { "/autoitem", "Auto Item theo danh sách" },
		new string[2] { "/tsn", "Mở menu tàn sát người" },
		new string[2] { "/cblockX", "Thêm id X vào danh sách né tàn sát" },
		new string[2] { "/caddX", "Thêm id X vào danh sách tàn sát theo ID" },
		new string[2] { "/cclanX", "Thêm ID Clan X vào danh sách tàn sát clan" },
		new string[2] { "/nrdX Y", "Auto vào map NRD Xsao khuY" },
		new string[2] { "/mvbtX", "Đếm nhặt đủ X mảnh vỡ bông tai, đủ sẽ thoát game" },
		new string[2] { "/mhbtX", "Đếm nhặt đủ X mảnh hồn bông tai, đủ sẽ thoát game" },
		new string[2] { "/mtsX", "Đếm nhặt đủ X mảnh áo quần thiên sứ, đủ sẽ thoát game" },
		new string[2] { "/quay", "Mở menu quay vòng quay thượng đế" },
		new string[2] { "/alg", "Auto đăng nhập lại khi mất kết nối" },
		new string[2] { "/xmp", "Mở Menu Xmap" },
		new string[2] { "/add", "Thêm quái ở vị trí đang trỏ vào vị trí quái tàn sát" },
		new string[2] { "/addt", "Thêm loại quái đang trỏ vào d/s loại quái tàn sát" },
		new string[2] { "/anhat", "Auto nhặt vật phẩm" },
		new string[2] { "/itm", "Lọc chỉ nhặt vật phẩm của bản thân" },
		new string[2] { "/addiX", "Thêm idItem X vào d/s chỉ nhặt item theo ID" },
		new string[2] { "/blockiX", "Thêm idItem X vào d/s không nhặt item" },
		new string[2] { "/addtiX", "Thêm loại item X vào d/s nhặt theo loại item" },
		new string[2] { "/pem1hit", "Tàn sát chỉ đánh quái sao cho quái còn 1HP" },
		new string[2] { "/mhpX", "Cài đặt chỉ tàn sát quái dưới X HP" },
		new string[2] { "/clri", "Xóa danh sách nhặt item" },
		new string[2] { "/cnn", "Lọc chỉ nhặt ngọc hồng, ngọc xanh" },
		new string[2] { "/ts", "Tàn sát quái" },
		new string[2] { "/nsq", "Tàn sát né siêu quái" },
		new string[2] { "/addmX", "Thêm quái id X vào d/s ID quái tàn sát" },
		new string[2] { "/addtmX", "Thêm loại quái X vào d/s tàn sát theo loại quái" },
		new string[2] { "/clrm", "Xóa danh sách quái tàn sát" },
		new string[2] { "/clrs", "Xóa danh sách skill tàn sát" },
		new string[2] { "/skill", "Thêm skill đang trỏ vào d/s skill tàn sát" },
		new string[2] { "/skillX", "Thêm skill ở vị trí X trong mục chỉ số vào d/s skill tàn sát" },
		new string[2] { "/abfX Y", "Auto dùng đậu thần khi HP, KI lần lượt dưới X%, Y%" },
		new string[2] { "/xmpX", "Auto di chuyển tới map id X" },
		new string[2] { "/csb", "Bật/Tắt sử dụng capsule thường khi Xmap" },
		new string[2] { "/csdb", "Bật/Tắt sử dụng capsule đặc biệt khi Xmap" },
		new string[2] { "/kmtX", "Khóa mục tiêu ID X" }
	};

	internal static HintCommand gI { get; } = new HintCommand();


	internal int maxWidth => (mGraphics.zoomLevel > 1) ? 280 : 350;

	internal int lenghtHintsShow => (hints.Count < 10) ? hints.Count : 10;

	private HintCommand()
	{
		hints_default = hints_default.OrderBy((string[] h) => h[0]).ToList();
	}

	public void show()
	{
		isShow = true;
		selectedIndex = 0;
		scrollValue = 0;
		chatBack = null;
		hints = hints_default;
	}

	internal void update()
	{
		isShow = ChatTextField.gI().isShow;
		if (!isShow)
		{
			hints = null;
			return;
		}
		TField tfChat = ChatTextField.gI().tfChat;
		if (tfChat.getText().Length == 0 || tfChat.getText()[0] != '/')
		{
			chatBack = null;
			isShow = false;
			return;
		}
		if (chatBack != tfChat.getText())
		{
			selectedIndex = 0;
			scrollValue = 0;
			chatBack = tfChat.getText();
			string cmd = chatBack.Substring(1);
			hints = hints_default.FindAll((string[] h) => h[0].Contains(cmd) || h[1].Contains(cmd));
		}
		if (GameCanvas.keyPressed[22])
		{
			selectedIndex++;
			if (selectedIndex > hints.Count - 1)
			{
				selectedIndex = 0;
			}
			GameCanvas.keyPressed[22] = false;
			GameCanvas.clearKeyPressed();
			GameCanvas.clearKeyHold();
		}
		else if (GameCanvas.keyPressed[21])
		{
			selectedIndex--;
			if (selectedIndex < 0)
			{
				selectedIndex = hints.Count - 1;
			}
			GameCanvas.keyPressed[21] = false;
			GameCanvas.clearKeyPressed();
			GameCanvas.clearKeyHold();
		}
		else if (GameCanvas.keyPressed[16])
		{
			if (chatBack != hints[selectedIndex][0])
			{
				tfChat.setText(hints[selectedIndex][0]);
			}
			GameCanvas.keyPressed[16] = false;
			GameCanvas.clearKeyPressed();
			GameCanvas.clearKeyHold();
		}
		if (selectedIndex >= scrollValue + 10)
		{
			scrollValue = selectedIndex - 9;
		}
		if (selectedIndex < scrollValue)
		{
			scrollValue = selectedIndex;
		}
	}

	internal void paint(mGraphics g)
	{
		if (isShow && !string.IsNullOrEmpty(chatBack))
		{
			ChatTextField chatTextField = ChatTextField.gI();
			height = (lenghtHintsShow + 1) * 10;
			width = ((GameCanvas.w - 10 > maxWidth) ? maxWidth : (GameCanvas.w - 10));
			int h = lenghtHintsShow * (height - 10) / hints.Count;
			x = (GameCanvas.w - width) / 2;
			y = chatTextField.tfChat.y - 40 - height;
			g.setColor(0, 0.5f);
			g.fillRect(x, y, width, height);
			g.setColor(0, 1f);
			g.fillRect(x, y, width, 10);
			int num = x + width - mFont.tahoma_7_white.getWidth("Nhấn Tab để lựa chọn") - 5;
			mFont.tahoma_7_white.drawString(g, "Nhấn Tab để lựa chọn", num, y, 0);
			g.setColor(16777215, 0.5f);
			g.fillRect(x, y + 10 - 1, width, 1);
			g.setColor(8618883, 0.75f);
			g.fillRect(x, y + 10 + 10 * (selectedIndex - scrollValue), width - 5, 10);
			g.setColor(16777215, 0.75f);
			g.fillRect(x, y + 10 + 10 * (selectedIndex - scrollValue), 2, 10);
			g.setColor(16777215, 0.75f);
			g.fillRect(x + width - 5, y + 10, 1, height - 10);
			g.setColor(16777215, 0.75f);
			g.fillRect(x + width - 3, y + 10 + scrollValue * (height - 10) / hints.Count, 2, h);
			for (int i = scrollValue; i < scrollValue + lenghtHintsShow; i++)
			{
				mFont.tahoma_7_white.drawString(g, hints[i][0] + " - " + hints[i][1], x + 5, y + 10 + 10 * (i - scrollValue), 0);
			}
		}
	}
}
