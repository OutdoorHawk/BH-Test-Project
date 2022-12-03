using System;
using BH_Test_Project.Code.Infrastructure.Services;
using Mirror;

namespace BH_Test_Project.Code.Infrastructure.Data
{
    public static class DateTimeReaderWriter
    {
        public static void WriteUIFactory(this NetworkWriter writer, IUIFactory uiFactory)
        {
            writer.Write(uiFactory);
        }
     
        /*public static IUIFactory ReadUIFactory(this NetworkReader reader)
        {
            return );
        }*/
    }
}
