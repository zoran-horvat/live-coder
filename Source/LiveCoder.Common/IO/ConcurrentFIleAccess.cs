using System.IO;
using System.Threading;
using LiveCoder.Common.Optional;

namespace LiveCoder.Common.IO
{
    public static class ConcurrentFIleAccess
    {
        public static FileStream OpenRead(this FileInfo file) =>
            File.OpenRead(file.FullName);

        public static Option<FileStream> TryOpenReadConcurrent(this FileInfo file)
        {
            int retries = 10;
            int waitMsec = 100;

            while (retries > 0)
            {
                try
                {
                    return File.OpenRead(file.FullName);
                }
                catch (IOException)
                {
                    Thread.Sleep(waitMsec);
                }
                retries -= 1;
            }

            return None.Value;
        }
    }
}
