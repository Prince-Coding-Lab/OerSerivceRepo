using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using Core.Models;
using Core.Enums;
using Core.Extensions;



namespace Core.Helpers
{
    public class ExceptionHelper
    {
        public string GetLogString(Exception ex, ErrorLevel level)
        {
            try
            {   
                StackTrace st = new StackTrace(ex, true);
                //Get the first stack frame
                StackFrame frame = st.GetFrame(0);

                //JsonConvert.SerializeObject();

              ExceptionLog exLog=   new ExceptionLog
                {
                    ExceptionLogId = new RandomSG().GetString(),
                    ExceptionType = ex.GetType().FullName.ToString(),                   
                    ExceptionInnerException = ex.InnerException == null ? "" : ex.InnerException.ToString(),
                    ExceptionMessage = ex.Message,
                    ExceptionSeverity = EnumExtensions.GetDescription(level),
                    ExceptionFileName = frame.GetFileName(), //Get the file name
                    ExceptionLineNumber = frame.GetFileLineNumber(),  //Get the line number
                    ExceptionColumnNumber = frame.GetFileColumnNumber(), //Get the column number                      
                    ExceptionMethodName = ex.TargetSite.ReflectedType.FullName // Get the method name

                };             
               

                string excep = $"ExceptionLogId:{exLog.ExceptionLogId}, ExceptionType:{exLog.ExceptionType}, ExceptionInnerException:{exLog.ExceptionInnerException}, ExceptionMessage:{exLog.ExceptionMessage}, ExceptionSeverity:{exLog.ExceptionSeverity}, ExceptionFileName:{exLog.ExceptionFileName}, ExceptionMethodName:{exLog.ExceptionMethodName}, ExceptionLineNumber:{exLog.ExceptionLineNumber}, ExceptionColumnNumber:{exLog.ExceptionColumnNumber}" ;

                return excep.Remove(0,1);


            }

            catch(Exception e)
            {
                throw e;
            }

        }

    }
}
