using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ScripterSharpCommon.UE;

namespace ScripterSharpCommon
{
    public unsafe class ObjectArray : IEnumerable<nint>
    {
        bool UseNew;
        FFixedUObjectArray* OldObjects;
        FChunkedFixedUObjectArray* NewObjects;

        public ObjectArray(nint ptr, bool useNew)
        {
            NewObjects = (FChunkedFixedUObjectArray*)ptr;
            OldObjects = (FFixedUObjectArray*)ptr;
            UseNew = useNew;
        }

        public int Num => UseNew ? NewObjects->NumElements : OldObjects->NumElements;

        public unsafe UObject* GetObjectById(int index) => UseNew ? NewObjects->GetObjectById(index) : OldObjects->GetObjectById(index);
        public unsafe nint GetObjectByIdAsIntPtr(int index) => UseNew ? (nint)NewObjects->GetObjectById(index) : (nint)OldObjects->GetObjectById(index);

        public IEnumerator<nint> GetEnumerator()
        {
            for (int i = 0; i < Num; i++)
            {
                var obj = GetObjectByIdAsIntPtr(i);
                if (obj != nint.Zero)
                    yield return obj;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
