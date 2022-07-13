using ESCPOS_NET;
using ESCPOS_NET.Emitters;
using ESCPOS_NET.Utilities;
using MilkTeaShop.Models;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace MilkTeaShop.Services
{
    //public interface IPrinter
    //{
    //    public string SelectMethod(int MethodID);
    //    public void Print(List<PrinterModel> result, string IP, int Port);
    //}
    //public class PrinterServices : IPrinter
    //{
    //    private string JustificationCenter()
    //    {
    //        return "" + (char)27/*escape */ + (char)97/*a*/ + (char)1/*start of heading*/;
    //    }

    //    private string JustificationLeft()
    //    {
    //        return "" + (char)27 + (char)97 + (char)0;
    //    }

    //    private string DoubleHeight()
    //    {
    //        return "" + (char)27 + (char)33 + (char)16;
    //    }

    //    private string DoubleWidth()
    //    {
    //        return "" + (char)27 + (char)33 + (char)32;
    //    }

    //    private string CancelDoubleHeightWidth()
    //    {
    //        return "" + (char)27 + (char)33 + (char)0;
    //    }

    //    private string SetColorRed()
    //    {
    //        return "" + (char)27 + (char)114 + (char)1;
    //    }

    //    private string SetColorBlack()
    //    {
    //        return "" + (char)27 + (char)114 + (char)0;
    //    }

    //    public string NewLine()
    //    {
    //        return "" + "\n";
    //    }
    //    public string SelectMethod(int MethodID)
    //    {
    //        switch (MethodID)
    //        {
    //            case 1:
    //                return JustificationCenter();
    //            case 2:
    //                return JustificationLeft();
    //            case 3:
    //                return DoubleHeight();
    //            case 4:
    //                return DoubleWidth();
    //            case 5:
    //                return CancelDoubleHeightWidth();
    //            case 6:
    //                return SetColorRed();
    //            case 7:
    //                return SetColorBlack();
    //            default:
    //                return CancelDoubleHeightWidth();
    //        }
    //    }

    //    //--print method
    //    public void Print(List<PrinterModel> result, string IP, int Port)
    //    {            
    //        Socket clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
    //        clientSocket.NoDelay = true;

    //        IPAddress ip = IPAddress.Parse(IP);
    //        IPEndPoint ipep = new IPEndPoint(ip, Port);
    //        clientSocket.Connect(ipep);
    //        Encoding enc = Encoding.ASCII;

    //        foreach (PrinterModel item in result)
    //        {
    //            var command = SelectMethod(item.PrintMethodID);
    //            byte[] commandBytes = Encoding.UTF8.GetBytes(command);
    //            byte[] contentBytes = Encoding.UTF8.GetBytes(item.Value);
    //            clientSocket.Send(commandBytes);

    //            if (item.IsNeedPrint)
    //            {
    //                clientSocket.Send(contentBytes);
    //                var n = NewLine();
    //                byte[] nBytes = Encoding.UTF8.GetBytes(n);
    //                clientSocket.Send(nBytes);
    //            }
    //        }

    //        // Line feed hexadecimal values
    //        byte[] bEsc = new byte[4];
    //        bEsc[0] = 0x0A;
    //        bEsc[1] = 0x0A;
    //        bEsc[2] = 0x0A;
    //        bEsc[3] = 0x0A;

    //        // Send the bytes over 
    //        clientSocket.Send(bEsc);

    //        clientSocket.Close();
    //    }      
    //}
    public interface IPrinter
    {
        void Print(List<PrinterModel> result, string IP, int Port);
    }
    public class PrinterServices : IPrinter
    {
        public void Print(List<PrinterModel> result, string IP, int Port)
        {  
            var printer = new NetworkPrinter(new NetworkPrinterSettings() { ConnectionString = $"{IP}:{Port}" });
            var e = new EPSON();
            var bytes = ByteSplicer.Combine(
                e.SetStyles(PrintStyle.FontB),
                e.CenterAlign(),               
                e.PrintLine("Bụi Tea"),
                e.PrintLine("93 Nguyễn Văn Tiết, Hiệp Thành, TDM, Bình Dương.")
                
              );
            
            printer.Write(
              bytes
            );
        }
    }
}
