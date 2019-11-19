using System;
using System.IO;
using System.Text;
using System.Threading;
using LiveCoder.Common.Optional;

namespace LiveCoder.Common.IO
{
    public static class ConcurrentFIleAccess
    {
        public static FileStream OpenRead(this FileInfo file) =>
            File.OpenRead(file.FullName);

        public static Option<FileStream> TryOpenReadConcurrent(this FileInfo file) =>
            RepeatUntilNoException(() =>File.OpenRead(file.FullName));

        public static Option<string[]> TryReadAllLines(this FileInfo file, Encoding encoding) =>
            RepeatUntilNoException(() => File.ReadAllLines(file.FullName, encoding));

        private static Option<T> RepeatUntilNoException<T>(Func<T> factory)
        {
            int retries = 10;
            int waitMsec = 100;

            while (retries > 0)
            {
                try
                {
                    return Option.Of(factory());
                }
                catch (IOException)
                {
                    Thread.Sleep(waitMsec);
                }
                retries -= 1;
            }

            return Option.None<T>();
        }
    }
}
