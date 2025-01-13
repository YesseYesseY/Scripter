using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ScripterSharpCommon.UE
{
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct FChunkedFixedUObjectArray
    {
        static int NumElementsPerChunk = 64 * 1024;
        FUObjectItem** Objects;
        FUObjectItem* PreAllocatedObjects;
        int MaxElements;
        public int NumElements;
        int MaxChunks;
        int NumChunks;

        public UObject* GetObjectById(int Index)
        {
            if (Index > NumElements || Index < 0) return null;

            int ChunkIndex = Index / NumElementsPerChunk;
            int WithinChunkIndex = Index % NumElementsPerChunk;

            if (ChunkIndex > NumChunks) return null;
            FUObjectItem* Chunk = Objects[ChunkIndex];
            if (Chunk is null) return null;

            var obj = (Chunk + WithinChunkIndex)->Object;

            return obj;
        }
    }
}
