﻿using System;
using System.IO;
using System.Reflection;
using System.Reflection.Emit;
using LLVM;

namespace CudaSharp
{
    public static class CudaSharp
    {
        static CudaSharp()
        {
            var extractTo = Path.GetFullPath("LLVM-3.3");
            if (File.Exists(extractTo))
                return;
            var file = File.Open(extractTo, FileMode.OpenOrCreate);
            var llvm34 = Assembly.GetExecutingAssembly().GetManifestResourceStream("CudaSharp.LLVM-3.4.dll");
            if (llvm34 == null)
                throw new Exception("Could not extract LLVM-3.4.dll");
            llvm34.CopyTo(file);
            file.Close();
        }

        public static OpCode[] UnsupportedInstructions
        {
            get { return Translator.UnsupportedInstructions; }
        }

        public static byte[] Translate(Action method) { return Translate(method.Method); }
        public static byte[] Translate<T1>(Action<T1> method) { return Translate(method.Method); }
        public static byte[] Translate<T1, T2>(Action<T1, T2> method) { return Translate(method.Method); }
        public static byte[] Translate<T1, T2, T3>(Action<T1, T2, T3> method) { return Translate(method.Method); }
        public static byte[] Translate<T1, T2, T3, T4>(Action<T1, T2, T3, T4> method) { return Translate(method.Method); }
        public static byte[] Translate<T1, T2, T3, T4, T5>(Action<T1, T2, T3, T4, T5> method) { return Translate(method.Method); }
        public static byte[] Translate<T1, T2, T3, T4, T5, T6>(Action<T1, T2, T3, T4, T5, T6> method) { return Translate(method.Method); }

        public static byte[] Translate(params MethodInfo[] methods)
        {
            var module = Translator.Translate(Context.Global, methods);
            return PInvokeHelper.EmitInMemory(module);
        }

        public static byte[] Translate(string targetCpu, params MethodInfo[] methods)
        {
            var module = Translator.Translate(Context.Global, methods);
            return PInvokeHelper.EmitInMemory(module, targetCpu);
        }
    }
}