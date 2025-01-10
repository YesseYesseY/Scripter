// TODO: Make these runtime options?
// #define UseTypeClassNames // Use UInt8 instead of byte, etc.
// #define UseIntPtrInsteadOfPointer
#define DynamicOffsets // Replace offset in fields with _obj.GetChildOffset()

using System.Text;
using ScripterSharp.UE;

namespace ScripterSharp
{
    public static unsafe class AdvancedDumper
    {
        private struct DumperField
        {
            public string Type;
            public string Name;

            public string Functionality()
            {
                return $"*({Type}*)_obj.GetPtrOffset(_{Name} == -1 ? _obj.GetChildOffset(\"{Name}\") : _{Name})";
            }
        }

        private struct DumperFunction
        {
            public DumperFunction()
            {
                ReturnParam = null;
                Params = new List<DumperField>();
                AllParams = new List<DumperField>();
                Name = "";
                FullName = "";
            }

            public string Name;
            public string FullName;

            public List<DumperField> Params;
            public List<DumperField> AllParams;
            public DumperField? ReturnParam;
        }

        private static UObject* UClassClass;
        private static UObject* UFunctionClass;
        private static UObject* UPropertyClass;
        private static UObject* UEnumClass;
        private static UObject* UStructClass;

        private static UObject* UObjectPropertyClass;
        private static UObject* UStructPropertyClass;
        private static UObject* UArrayPropertyClass;
        private static UObject* UEnumPropertyClass;
        private static UObject* UMapPropertyClass;
        private static UObject* UBytePropertyClass;
        private static Dictionary<nint, string> SimplePropertyTypes = new Dictionary<nint, string>();
        private static int PropertySize;

        const bool UseTypeClassNames = false; 
        public static unsafe void Dump(string path = "C:\\FN\\dump\\")
        {
            Directory.CreateDirectory($"{path}classes");
            Directory.CreateDirectory($"{path}enums");
            Directory.CreateDirectory($"{path}structs");
            UClassClass = Scripter.FindObject("Class /Script/CoreUObject.Class");
            UFunctionClass = Scripter.FindObject("Class /Script/CoreUObject.Function");
            UPropertyClass = Scripter.FindObject("Class /Script/CoreUObject.Property");
            UObjectPropertyClass = Scripter.FindObject("Class /Script/CoreUObject.ObjectProperty");
            UStructPropertyClass = Scripter.FindObject("Class /Script/CoreUObject.StructProperty");
            UArrayPropertyClass = Scripter.FindObject("Class /Script/CoreUObject.ArrayProperty");
            UEnumPropertyClass = Scripter.FindObject("Class /Script/CoreUObject.EnumProperty");
            UMapPropertyClass = Scripter.FindObject("Class /Script/CoreUObject.MapProperty");
            UBytePropertyClass = Scripter.FindObject("Class /Script/CoreUObject.ByteProperty");
            UEnumClass = Scripter.FindObject("Class /Script/CoreUObject.Enum");
            UStructClass = Scripter.FindObject("Class /Script/CoreUObject.Struct");

            SimplePropertyTypes.Add((nint)Scripter.FindObject("Class /Script/CoreUObject.DoubleProperty"), "double");
            SimplePropertyTypes.Add((nint)Scripter.FindObject("Class /Script/CoreUObject.FloatProperty"),  "float");
#if !UseTypeClassNames
            SimplePropertyTypes.Add((nint)Scripter.FindObject("Class /Script/CoreUObject.Int8Property"),   "sbyte");
            SimplePropertyTypes.Add((nint)Scripter.FindObject("Class /Script/CoreUObject.Int16Property"),  "short");
            SimplePropertyTypes.Add((nint)Scripter.FindObject("Class /Script/CoreUObject.IntProperty"),    "int");
            SimplePropertyTypes.Add((nint)Scripter.FindObject("Class /Script/CoreUObject.Int64Property"),  "long");
            SimplePropertyTypes.Add((nint)Scripter.FindObject("Class /Script/CoreUObject.UInt16Property"), "ushort");
            SimplePropertyTypes.Add((nint)Scripter.FindObject("Class /Script/CoreUObject.UInt32Property"), "uint");
            SimplePropertyTypes.Add((nint)Scripter.FindObject("Class /Script/CoreUObject.UInt64Property"), "ulong");
#else
            SimplePropertyTypes.Add((nint)Scripter.FindObject("Class /Script/CoreUObject.Int8Property"), "Int8");
            SimplePropertyTypes.Add((nint)Scripter.FindObject("Class /Script/CoreUObject.Int16Property"), "Int16");
            SimplePropertyTypes.Add((nint)Scripter.FindObject("Class /Script/CoreUObject.IntProperty"), "Int32");
            SimplePropertyTypes.Add((nint)Scripter.FindObject("Class /Script/CoreUObject.Int64Property"), "Int64");
            SimplePropertyTypes.Add((nint)Scripter.FindObject("Class /Script/CoreUObject.UInt16Property"), "UInt16");
            SimplePropertyTypes.Add((nint)Scripter.FindObject("Class /Script/CoreUObject.UInt32Property"), "UInt32");
            SimplePropertyTypes.Add((nint)Scripter.FindObject("Class /Script/CoreUObject.UInt64Property"), "UInt64");
#endif

            //SimplePropertyTypes.Add((nint)Scripter.FindObject("Class /Script/CoreUObject.ByteProperty"),   "byte");
            SimplePropertyTypes.Add((nint)Scripter.FindObject("Class /Script/CoreUObject.BoolProperty"),   "bool");
            SimplePropertyTypes.Add((nint)Scripter.FindObject("Class /Script/CoreUObject.NameProperty"),   "FName");
            //SimplePropertyTypes.Add((nint)Scripter.FindObject("Class /Script/CoreUObject.DelegateProperty"), "DelegateProperty");
            //SimplePropertyTypes.Add((nint)Scripter.FindObject("Class /Script/CoreUObject.MulticastDelegateProperty"),   "MulticastDelegateProperty");
            //SimplePropertyTypes.Add((nint)Scripter.FindObject("Class /Script/CoreUObject.MulticastInlineDelegateProperty"),   "MulticastInlineDelegateProperty");
            //SimplePropertyTypes.Add((nint)Scripter.FindObject("Class /Script/CoreUObject.MulticastSparseDelegateProperty"),   "MulticastSparseDelegateProperty");
            SimplePropertyTypes.Add((nint)Scripter.FindObject("Class /Script/CoreUObject.StrProperty"),   "FString");
            
            PropertySize = ((UStruct*)UPropertyClass)->PropertiesSize;
            Scripter.Print($"PropertySize is {PropertySize}");



            foreach (UObject* Obj in
#if false
                Scripter.Objects
#else
                new UObject*[] {
                    //Scripter.FindObject("Class /Script/FortniteGame.FortPlayerPawnAthena"),
                    Scripter.FindObject("Class /Script/FortniteGame.FortPlayerPawn"),
                    //Scripter.FindObject("Class /Script/FortniteGame.FortPawn"),
                    //Scripter.FindObject("Enum /Script/FortniteGame.EFortResourceLevel"),
                    //Scripter.FindObject("Enum /Script/FortniteGame.EFortTeam"),
                    //Scripter.FindObject("Enum /Script/FortniteGame.EFortPlaylistType"),
                    //Scripter.FindObject("Enum /Script/FortniteGame.EAthenaGamePhase"),
                    //Scripter.FindObject("ScriptStruct /Script/CoreUObject.Guid"),
                    Scripter.FindObject("Class /Script/Engine.GameplayStatics"),
                }
#endif
                )
            {
                if (Obj == null) continue;
                StringBuilder sb = new StringBuilder();
                string folder = "";
                string ObjName = Obj->GetName();
                if (Obj->ClassPrivate->IsA(UEnumClass))
                {
                    folder = "enums";
                    var ObjAsEnum = (UEnum*)Obj;

                    sb.Append("// ").AppendLine(Obj->GetFullName());
                    sb.Append("\nenum ").Append(ObjName).Append("\n{\n");
                    for (int i = 0; i < ObjAsEnum->Names.ArrayNum; i++)
                    {
                        var EnumThing = ObjAsEnum->Names[i];
                        var ThingName = EnumThing.First.ToString();
                        if (ThingName.Contains("::"))
                            ThingName = ThingName.Substring($"{ObjName}::".Length);
                        sb.Append("\t").Append(ThingName).Append(" = ").Append(EnumThing.Second).Append(",\n");
                    }

                    sb.Append("}");
                }
                else if (Obj->ClassPrivate->IsA(UStructClass) && !Obj->ClassPrivate->IsA(UFunctionClass))
                {
                    var ObjAsStruct = (UStruct*)Obj;

                    var IsAClass = Obj->ClassPrivate->IsA(UClassClass);
                    var ClassName = 'U' + ObjName;

                    sb.Append("// ").AppendLine(Obj->GetFullName());
                    sb.Append("// ").AppendLine(IsAClass ? "UClass" : "UStruct");
                    sb.AppendLine($"[StructLayout(LayoutKind.Explicit, Size = {ObjAsStruct->PropertiesSize})]");
                    sb.Append("public struct ").Append(ClassName);
                    folder = IsAClass ? "classes" : "structs";
                    if (ObjAsStruct->SuperStruct != null)
                    {
                        sb.Append(" // : U").Append(ObjAsStruct->SuperStruct->GetName());
                    }
                    sb.Append("\n{\n");
#if DynamicOffsets
                    sb.Append("\t[FieldOffset(0)] private UObject _obj;\n\n");
#endif
                    List<DumperField> fields = new List<DumperField>();
                    List<DumperFunction> funcs = new List<DumperFunction>();

                    for (var Child = ObjAsStruct->Children; Child != null; Child = Child->Next)
                    {
                        var IsAProperty = Child->ClassPrivate->IsA(UPropertyClass);
                        var ChildName = Child->GetName();

                        if (IsAProperty)
                        {
                            var (ChildType, Notes) = GetType((UObject*)Child);

                            fields.Add(new DumperField
                            {
                                Name = ChildName,
                                Type = ChildType
                            });
                        }
                        else
                        {
                            var DumperFunc = new DumperFunction()
                            {
                                Name = ChildName,
                                FullName = Child->GetFullName()
                            };
                            for (var FuncChild = ((UStruct*)Child)->Children; FuncChild != null; FuncChild = FuncChild->Next)
                            {
                                var FuncChildAsProperty = (UProperty*)FuncChild;
                                var (FuncChildType, FuncChildNotes) = GetType((UObject*)FuncChild);
                                var field = new DumperField
                                {
                                    Name = FuncChild->GetName(),
                                    Type = FuncChildType
                                };
                                DumperFunc.AllParams.Add(field);
                                if (FuncChildAsProperty->PropertyFlags.HasFlag(EPropertyFlags.CPF_ReturnParm))
                                {
                                    DumperFunc.ReturnParam = field;
                                }
                                else
                                {
                                    DumperFunc.Params.Add(field);
                                }
                            }

                            funcs.Add(DumperFunc);
                        }
                    }

                    // TODO: MAKE THIS READABLE

                    foreach (var field in fields)
                    {
                        sb.Append($"\tprivate static int _{field.Name} = -1;\n"); // TODO: Remove line for setting NoFunctionality
                        sb.Append($"\tpublic {field.Type} {field.Name}"); // TODO: Add ; for setting NoFucntionality
                        sb.Append("\n\t{{\n\t\tget => {field.Functionality()};\n\t\tset => {field.Functionality()} = value;\n\t}}\n"); // TODO: Remove line for setting NoFunctionality
                    }

                    sb.Append('\n');

                    foreach (var func in funcs)
                    {
                        sb.Append($"\t// ----FUNCTION_{func.Name}----\n");
                        if (func.AllParams.Count > 0)
                        {
                            sb.Append("\t[StructLayout(LayoutKind.Sequential)]\n");
                            sb.Append($"\tprivate struct Params_{func.Name}\n\t{{\n");
                            foreach (var param in func.Params)
                            {
                                sb.Append($"\t\tpublic {param.Type} {param.Name};\n");
                            }
                            if (func.ReturnParam != null)
                                sb.Append($"\t\tpublic {func.ReturnParam.Value.Type} {func.ReturnParam.Value.Name};\n");
                            sb.Append("\t}\n");
                        }
                        sb.Append($"\tprivate static UObject* Func_{func.Name};\n");
                        var returnType = func.ReturnParam == null ? "void" : func.ReturnParam.Value.Type;
                        sb.Append($"\tpublic {returnType} {func.Name}(");
                        for (int i = 0; i < func.Params.Count; i++)
                        {
                            var param = func.Params[i];
                            sb.Append($"{param.Type} {param.Name}");
                            if (func.Params.Count - 1 != i) sb.Append(", ");
                        }
                        sb.Append(')');
                        if (false) // For setting NoFunctionality
                        {
                            sb.Append(';');
                        }
                        else
                        {
                            sb.Append("\n\t{\n");
                            if (func.AllParams.Count > 0)
                            {
                                sb.Append($"\t\tvar args = new Params_{func.Name}();\n");
                                foreach (var param in func.Params)
                                {
                                    sb.Append($"\t\targs.{param.Name} = {param.Name};\n");
                                }
                            }
                            sb.Append($"\t\tif (Func_{func.Name} == null) Func_{func.Name} = Scripter.FindObject(\"{func.FullName}\");\n");
                            sb.Append($"\t\t_obj.ProcessEvent(Func_{func.Name}, ");
                            if (func.AllParams.Count > 0)
                                sb.Append("&args);\n");
                            else
                                sb.Append("null);\n");

                            if (func.ReturnParam != null)
                                sb.Append($"\t\treturn args.{func.ReturnParam.Value.Name};\n");
                            sb.Append("\t}");
                        }
                        sb.Append('\n');
                    }

                    sb.Append("}");
                }

                var finalStr = sb.ToString();
                if (finalStr != "")
                    File.WriteAllText($"{path}{folder}\\{ObjName}.cs", finalStr);
            }

            Scripter.Print("Finished dumping :)");
        }

        public static (string, string?) GetType(UObject* Child)
        {
            var ChildType = SimplePropertyTypes.GetValueOrDefault((nint)Child->ClassPrivate, "UNKNOWN_TYPE");
            string? Notes = null;

            if (ChildType == "UNKNOWN_TYPE")
            {
                if (Child->ClassPrivate->IsA(UObjectPropertyClass))
                {
                    var ObjPropType = (*(UObject**)Child->GetPtrOffset(PropertySize))->GetName();
#if UseIntPtrInsteadOfPointer
                    Notes = $"U{ObjPropType}*";
#if UseTypeClassNames
                    ChildType = $"IntPtr";
#else
                    ChildType = $"nint";
#endif
#else
                    ChildType = $"U{ObjPropType}*";
#endif
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
#if !UseTypeClassNames
                        ChildType = "byte";
#else
                        ChildType = "UInt8";
#endif
                    }
                }
                //else if (Child->ClassPrivate->IsA(UMapPropertyClass))
                //{
                //    var Key = *(UObject**)Child->GetPtrOffset(PropertySize);
                //    var Value = *(UObject**)Child->GetPtrOffset(PropertySize + 8);
                //    var (KeyType, KeyNotes) = GetType(Key);
                //    var (ValueType, ValueNotes) = GetType(Value);
                //    ChildType = $"TMap<{KeyType},{ValueType}{(KeyNotes == null ? "" : $" /* {KeyNotes}, {ValueNotes} */")}>";
                //}
                else if (Child->ClassPrivate->IsA(UArrayPropertyClass))
                {
                    var ObjPropType = *(UObject**)Child->GetPtrOffset(PropertySize);
                    var (CT, CTNotes) = GetType(ObjPropType);
                    ChildType = $"TArray<{CT}{(CTNotes == null ? "" : $" /* {CTNotes} */")}>";
                }
            }

            return (ChildType, Notes);
        }
    }
}
/*
TODO:
Bitfield in BoolProperty,
Enum type ( enum Name : {this type} )
Add TMap<T, T2> to the main project
Dumper settings
Checkout classes below:
    Class /Script/CoreUObject.NumericProperty
    Class /Script/CoreUObject.ClassProperty
    Class /Script/CoreUObject.InterfaceProperty
    Class /Script/CoreUObject.LazyObjectProperty
    Class /Script/CoreUObject.SetProperty
    Class /Script/CoreUObject.SoftObjectProperty
    Class /Script/CoreUObject.SoftClassProperty
    Class /Script/CoreUObject.WeakObjectProperty
    Class /Script/CoreUObject.TextProperty
 */