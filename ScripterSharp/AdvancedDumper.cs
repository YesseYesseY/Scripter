using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ScripterSharp.UE;

namespace ScripterSharp
{
    public static unsafe class AdvancedDumper
    {
        private static UObject* UClassClass;
        private static UObject* UFunctionClass;
        private static UObject* UPropertyClass;
        private static UObject* UObjectPropertyClass;
        private static UObject* UStructPropertyClass;
        private static UObject* UArrayPropertyClass;
        private static UObject* UEnumPropertyClass;
        private static UObject* UMapPropertyClass;
        private static UObject* UBytePropertyClass;
        private static Dictionary<nint, string> SimplePropertyTypes = new Dictionary<nint, string>();
        private static int PropertySize;
        public static unsafe void Dump(string path = "C:\\FN\\dump\\")
        {
            Directory.CreateDirectory(path);
            UClassClass = Scripter.FindObject("Class /Script/CoreUObject.Class");
            UFunctionClass = Scripter.FindObject("Class /Script/CoreUObject.Function");
            UPropertyClass = Scripter.FindObject("Class /Script/CoreUObject.Property");
            UObjectPropertyClass = Scripter.FindObject("Class /Script/CoreUObject.ObjectProperty");
            UStructPropertyClass = Scripter.FindObject("Class /Script/CoreUObject.StructProperty");
            UArrayPropertyClass = Scripter.FindObject("Class /Script/CoreUObject.ArrayProperty");
            UEnumPropertyClass = Scripter.FindObject("Class /Script/CoreUObject.EnumProperty");
            UMapPropertyClass = Scripter.FindObject("Class /Script/CoreUObject.MapProperty");
            UBytePropertyClass = Scripter.FindObject("Class /Script/CoreUObject.ByteProperty");

            SimplePropertyTypes.Add((nint)Scripter.FindObject("Class /Script/CoreUObject.DoubleProperty"), "double");
            SimplePropertyTypes.Add((nint)Scripter.FindObject("Class /Script/CoreUObject.FloatProperty"),  "float");
            SimplePropertyTypes.Add((nint)Scripter.FindObject("Class /Script/CoreUObject.Int8Property"),   "sbyte");
            SimplePropertyTypes.Add((nint)Scripter.FindObject("Class /Script/CoreUObject.Int16Property"),  "short");
            SimplePropertyTypes.Add((nint)Scripter.FindObject("Class /Script/CoreUObject.IntProperty"),    "int");
            SimplePropertyTypes.Add((nint)Scripter.FindObject("Class /Script/CoreUObject.Int64Property"),  "long");
            //SimplePropertyTypes.Add((nint)Scripter.FindObject("Class /Script/CoreUObject.ByteProperty"),   "byte");
            SimplePropertyTypes.Add((nint)Scripter.FindObject("Class /Script/CoreUObject.UInt16Property"), "ushort");
            SimplePropertyTypes.Add((nint)Scripter.FindObject("Class /Script/CoreUObject.UInt32Property"), "uint");
            SimplePropertyTypes.Add((nint)Scripter.FindObject("Class /Script/CoreUObject.UInt64Property"), "ulong");
            SimplePropertyTypes.Add((nint)Scripter.FindObject("Class /Script/CoreUObject.BoolProperty"),   "bool");
            SimplePropertyTypes.Add((nint)Scripter.FindObject("Class /Script/CoreUObject.NameProperty"),   "FName");
            SimplePropertyTypes.Add((nint)Scripter.FindObject("Class /Script/CoreUObject.DelegateProperty"), "DelegateProperty");
            SimplePropertyTypes.Add((nint)Scripter.FindObject("Class /Script/CoreUObject.MulticastDelegateProperty"),   "MulticastDelegateProperty");
            SimplePropertyTypes.Add((nint)Scripter.FindObject("Class /Script/CoreUObject.MulticastInlineDelegateProperty"),   "MulticastInlineDelegateProperty");
            SimplePropertyTypes.Add((nint)Scripter.FindObject("Class /Script/CoreUObject.MulticastSparseDelegateProperty"),   "MulticastSparseDelegateProperty");
            
            PropertySize = ((UStruct*)UPropertyClass)->PropertiesSize;
            Scripter.Print($"PropertySize is {PropertySize}");
            
            foreach (UObject* Obj in
#if true
                Scripter.Objects
#else
                new UObject*[] {
                    Scripter.FindObject("Class /Script/FortniteGame.FortPlayerPawnAthena"),
                    Scripter.FindObject("Class /Script/FortniteGame.FortPlayerPawn"),
                    Scripter.FindObject("Class /Script/FortniteGame.FortPawn"),
                }
#endif
                )
            {
                StringBuilder sb = new StringBuilder();
                string ObjName = Obj->GetName();
                if (Obj->ClassPrivate->IsA(UClassClass))
                {
                    Scripter.Print(Obj->GetFullName());
                    var ObjAsStruct = (UStruct*)Obj;

                    sb.Append("// ").AppendLine(Obj->GetFullName());
                    sb.Append("// Size: ").Append(ObjAsStruct->PropertiesSize);
                    var ClassName = 'U' + ObjName;
                    sb.Append("\nclass ").Append(ClassName);
                    if (ObjAsStruct->SuperStruct != null)
                    {
                        sb.Append(" : U").Append(ObjAsStruct->SuperStruct->GetName());
                    }
                    sb.Append("\n{\n");

                    for (var Child = ObjAsStruct->Children; Child != null; Child = Child->Next)
                    {
                        var ChildName = Child->GetName();
                        sb.Append('\t');
                        if (Child->ClassPrivate->IsA(UFunctionClass))
                        {
                            sb.Append("void ").Append(ChildName).Append("();");
                        }
                        else if (Child->ClassPrivate->IsA(UPropertyClass))
                        {
                            var ChildAsProperty = (UProperty*)Child;
                            var ChildType = GetType((UObject*)Child);

                            sb.Append(ChildType).Append(" ").Append(ChildName).Append("; // Size: ").Append(ChildAsProperty->ElementSize)
                                .Append(", Offset: ").Append(ChildAsProperty->Offset_Internal).Append(", ClassPrivate: ").Append(Child->ClassPrivate->GetName());
                        }
                        else
                        {
                            sb.Append("wtf is this?");
                        }
                        sb.Append('\n');
                    }

                    sb.Append("};");
                }

                var finalStr = sb.ToString();
                if (finalStr != "")
                    File.WriteAllText($"{path}{ObjName}.cs", finalStr);
            }

            Scripter.Print("Finished dumping :)");
        }

        public static string GetType(UObject* Child)
        {
            var ChildType = SimplePropertyTypes.GetValueOrDefault((nint)Child->ClassPrivate, "UNKNOWN_TYPE");

            if (ChildType == "UNKNOWN_TYPE")
            {
                if (Child->ClassPrivate->IsA(UObjectPropertyClass))
                {
                    var ObjPropType = (*(UObject**)Child->GetPtrOffset(PropertySize))->GetName();
                    ChildType = $"U{ObjPropType}";
                }
                else if (Child->ClassPrivate->IsA(UEnumPropertyClass))
                {
                    var ObjPropType = (*(UObject**)Child->GetPtrOffset(PropertySize + 8))->GetName();
                    ChildType = $"{ObjPropType}";
                }
                else if (Child->ClassPrivate->IsA(UStructPropertyClass))
                {
                    var ObjPropType = (*(UObject**)Child->GetPtrOffset(PropertySize))->GetName();
                    ChildType = $"F{ObjPropType}";
                }
                else if (Child->ClassPrivate->IsA(UBytePropertyClass))
                {
                    var Enum = *(UObject**)Child->GetPtrOffset(PropertySize);
                    if (Enum != null)
                    {
                        ChildType = $"TEnumAsByte<{Enum->GetName()}>";
                    }
                    else
                    {
                        ChildType = "byte";
                    }
                }
                else if (Child->ClassPrivate->IsA(UMapPropertyClass))
                {
                    var Key = *(UObject**)Child->GetPtrOffset(PropertySize);
                    var Value = *(UObject**)Child->GetPtrOffset(PropertySize + 8);
                    ChildType = $"TMap<{GetType(Key)},{GetType(Value)}>";
                }
                else if (Child->ClassPrivate->IsA(UArrayPropertyClass))
                {
                    var ObjPropType = *(UObject**)Child->GetPtrOffset(PropertySize);
                    ChildType = $"TArray<{GetType(ObjPropType)}>";
                }
            }

            return ChildType;
        }
    }
}
/*
TODO:
Bitfield in BoolProperty,
Function return values + params
Enums
Structs
Checkout classes below:
    Class /Script/CoreUObject.NumericProperty
    Class /Script/CoreUObject.ClassProperty
    Class /Script/CoreUObject.InterfaceProperty
    Class /Script/CoreUObject.LazyObjectProperty
    Class /Script/CoreUObject.MapProperty
    Class /Script/CoreUObject.SetProperty
    Class /Script/CoreUObject.SoftObjectProperty
    Class /Script/CoreUObject.SoftClassProperty
    Class /Script/CoreUObject.StrProperty
    Class /Script/CoreUObject.WeakObjectProperty
    Class /Script/CoreUObject.TextProperty
 */