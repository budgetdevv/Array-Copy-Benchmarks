# Array-Copy-Benchmarks
Benchmarks for different array copying APIs

``` ini

BenchmarkDotNet=v0.13.1, OS=Windows 10.0.22000
Intel Core i7-8700K CPU 3.70GHz (Coffee Lake), 1 CPU, 12 logical and 6 physical cores
.NET SDK=7.0.100-rc.1.22431.12
  [Host]     : .NET 7.0.0 (7.0.22.42610), X64 RyuJIT
  DefaultJob : .NET 7.0.0 (7.0.22.42610), X64 RyuJIT

```
|             Method |       Mean |    Error |   StdDev | Code Size |
|------------------- |-----------:|---------:|---------:|----------:|
|          ArrayCopy |   685.6 μs | 12.42 μs | 10.37 μs |     284 B |
| SpanCopyToNoChecks |   695.1 μs | 13.47 μs | 12.60 μs |     333 B |
|         SpanCopyTo |   771.7 μs | 15.29 μs | 16.36 μs |     352 B |
|            ToArray | 1,579.4 μs | 12.37 μs | 10.97 μs |     259 B |

# Disassembly

## .NET 7.0.0 (7.0.22.42610), X64 RyuJIT
```assembly
; Bench.ArrayCopy()
       push      rsi
       sub       rsp,20
       mov       rcx,20D16C06358
       mov       rcx,[rcx]
       mov       rdx,20D16C06360
       mov       rsi,[rdx]
       mov       rdx,rsi
       mov       r8d,989680
       call      qword ptr [7FFE2D6D9018]
       mov       rax,rsi
       add       rsp,20
       pop       rsi
       ret
; Total bytes of code 55
```
```assembly
; System.Array.Copy(System.Array, System.Array, Int32)
       push      rdi
       push      rsi
       push      rbp
       push      rbx
       sub       rsp,38
       mov       rsi,rcx
       mov       rdi,rdx
       mov       ebx,r8d
       test      rsi,rsi
       je        near ptr M01_L03
       test      rdi,rdi
       je        near ptr M01_L04
       mov       rcx,[rsi]
       cmp       rcx,[rdi]
       jne       short M01_L01
       cmp       dword ptr [rcx+4],18
       ja        short M01_L01
       cmp       ebx,[rsi+8]
       ja        short M01_L01
       cmp       ebx,[rdi+8]
       ja        short M01_L01
       mov       r8d,ebx
       movzx     edx,word ptr [rcx]
       imul      r8,rdx
       lea       rdx,[rsi+10]
       add       rdi,10
       test      dword ptr [rcx],1000000
       je        short M01_L00
       cmp       r8,4000
       ja        short M01_L02
       mov       rcx,rdi
       add       rsp,38
       pop       rbx
       pop       rbp
       pop       rsi
       pop       rdi
       jmp       near ptr System.Buffer.__BulkMoveWithWriteBarrier(Byte ByRef, Byte ByRef, UIntPtr)
M01_L00:
       mov       rcx,rdi
       add       rsp,38
       pop       rbx
       pop       rbp
       pop       rsi
       pop       rdi
       jmp       qword ptr [7FFE2D6D99F0]
M01_L01:
       mov       rcx,rsi
       xor       edx,edx
       call      qword ptr [7FFE2D6D91C8]
       mov       ebp,eax
       mov       rcx,rdi
       xor       edx,edx
       call      qword ptr [7FFE2D6D91C8]
       mov       r9d,eax
       mov       [rsp+20],ebx
       xor       edx,edx
       mov       [rsp+28],edx
       mov       edx,ebp
       mov       r8,rdi
       mov       rcx,rsi
       call      qword ptr [7FFE2D6D9048]
       nop
       add       rsp,38
       pop       rbx
       pop       rbp
       pop       rsi
       pop       rdi
       ret
M01_L02:
       mov       rcx,rdi
       add       rsp,38
       pop       rbx
       pop       rbp
       pop       rsi
       pop       rdi
       jmp       qword ptr [7FFE2D6D98D0]
M01_L03:
       mov       ecx,41
       call      qword ptr [7FFE2D6E1360]
       int       3
M01_L04:
       mov       ecx,43
       call      qword ptr [7FFE2D6E1360]
       int       3
; Total bytes of code 229
```

## .NET 7.0.0 (7.0.22.42610), X64 RyuJIT
```assembly
; Bench.SpanCopyToNoChecks()
       push      rsi
       sub       rsp,20
       mov       rcx,26733806358
       mov       rdx,[rcx]
       add       rdx,10
       mov       rcx,26733806360
       mov       rsi,[rcx]
       mov       rcx,rsi
       add       rcx,10
       mov       r8d,989680
       call      qword ptr [7FFE2D6D99F0]
       mov       rax,rsi
       add       rsp,20
       pop       rsi
       ret
; Total bytes of code 63
```
```assembly
; System.Buffer.Memmove(Byte ByRef, Byte ByRef, UIntPtr)
       vzeroupper
       mov       rax,rcx
       sub       rax,rdx
       cmp       rax,r8
       jae       short M01_L01
M01_L00:
       cmp       rcx,rdx
       je        near ptr M01_L09
       jmp       near ptr M01_L11
M01_L01:
       mov       rax,rdx
       sub       rax,rcx
       cmp       rax,r8
       jb        short M01_L00
       lea       rax,[rdx+r8]
       lea       r9,[rcx+r8]
       cmp       r8,10
       jbe       short M01_L04
       cmp       r8,40
       ja        near ptr M01_L07
M01_L02:
       vmovupd   xmm0,[rdx]
       vmovupd   [rcx],xmm0
       cmp       r8,20
       jbe       short M01_L03
       vmovupd   xmm0,[rdx+10]
       vmovupd   [rcx+10],xmm0
       cmp       r8,30
       jbe       short M01_L03
       vmovupd   xmm0,[rdx+20]
       vmovupd   [rcx+20],xmm0
M01_L03:
       vmovupd   xmm0,[rax+0FFF0]
       vmovupd   [r9+0FFF0],xmm0
       jmp       short M01_L09
M01_L04:
       test      r8b,18
       je        short M01_L05
       mov       r8,[rdx]
       mov       [rcx],r8
       mov       rdx,[rax+0FFF8]
       mov       [r9+0FFF8],rdx
       jmp       short M01_L09
M01_L05:
       test      r8b,4
       je        short M01_L06
       mov       r8d,[rdx]
       mov       [rcx],r8d
       mov       edx,[rax+0FFFC]
       mov       [r9+0FFFC],edx
       jmp       short M01_L09
M01_L06:
       test      r8,r8
       je        short M01_L09
       movzx     edx,byte ptr [rdx]
       mov       [rcx],dl
       test      r8b,2
       je        short M01_L09
       movsx     r8,word ptr [rax+0FFFE]
       mov       [r9+0FFFE],r8w
       jmp       short M01_L09
M01_L07:
       cmp       r8,800
       ja        short M01_L11
       mov       r10,r8
       shr       r10,6
M01_L08:
       vmovdqu   ymm0,ymmword ptr [rdx]
       vmovdqu   ymmword ptr [rcx],ymm0
       vmovdqu   ymm0,ymmword ptr [rdx+20]
       vmovdqu   ymmword ptr [rcx+20],ymm0
       add       rcx,40
       add       rdx,40
       dec       r10
       je        short M01_L10
       jmp       short M01_L08
M01_L09:
       ret
M01_L10:
       and       r8,3F
       cmp       r8,10
       ja        near ptr M01_L02
       vmovupd   xmm0,[rax+0FFF0]
       vmovupd   [r9+0FFF0],xmm0
       jmp       short M01_L09
M01_L11:
       jmp       qword ptr [7FFE2D6D9A08]
; Total bytes of code 270
```

## .NET 7.0.0 (7.0.22.42610), X64 RyuJIT
```assembly
; Bench.SpanCopyTo()
       push      rsi
       sub       rsp,20
       mov       rcx,27082806358
       mov       rcx,[rcx]
       lea       rdx,[rcx+10]
       mov       ecx,[rcx+8]
       mov       r8,27082806360
       mov       rsi,[r8]
       mov       r8,rsi
       lea       rax,[r8+10]
       mov       r8d,[r8+8]
       cmp       ecx,r8d
       ja        short M00_L00
       mov       r8d,ecx
       mov       rcx,rax
       call      qword ptr [7FFE2D6E99F0]
       mov       rax,rsi
       add       rsp,20
       pop       rsi
       ret
M00_L00:
       call      qword ptr [7FFE2D6F1060]
       int       3
; Total bytes of code 82
```
```assembly
; System.Buffer.Memmove(Byte ByRef, Byte ByRef, UIntPtr)
       vzeroupper
       mov       rax,rcx
       sub       rax,rdx
       cmp       rax,r8
       jae       short M01_L01
M01_L00:
       cmp       rcx,rdx
       je        near ptr M01_L09
       jmp       near ptr M01_L11
M01_L01:
       mov       rax,rdx
       sub       rax,rcx
       cmp       rax,r8
       jb        short M01_L00
       lea       rax,[rdx+r8]
       lea       r9,[rcx+r8]
       cmp       r8,10
       jbe       short M01_L04
       cmp       r8,40
       ja        near ptr M01_L07
M01_L02:
       vmovupd   xmm0,[rdx]
       vmovupd   [rcx],xmm0
       cmp       r8,20
       jbe       short M01_L03
       vmovupd   xmm0,[rdx+10]
       vmovupd   [rcx+10],xmm0
       cmp       r8,30
       jbe       short M01_L03
       vmovupd   xmm0,[rdx+20]
       vmovupd   [rcx+20],xmm0
M01_L03:
       vmovupd   xmm0,[rax+0FFF0]
       vmovupd   [r9+0FFF0],xmm0
       jmp       short M01_L09
M01_L04:
       test      r8b,18
       je        short M01_L05
       mov       r8,[rdx]
       mov       [rcx],r8
       mov       rdx,[rax+0FFF8]
       mov       [r9+0FFF8],rdx
       jmp       short M01_L09
M01_L05:
       test      r8b,4
       je        short M01_L06
       mov       r8d,[rdx]
       mov       [rcx],r8d
       mov       edx,[rax+0FFFC]
       mov       [r9+0FFFC],edx
       jmp       short M01_L09
M01_L06:
       test      r8,r8
       je        short M01_L09
       movzx     edx,byte ptr [rdx]
       mov       [rcx],dl
       test      r8b,2
       je        short M01_L09
       movsx     r8,word ptr [rax+0FFFE]
       mov       [r9+0FFFE],r8w
       jmp       short M01_L09
M01_L07:
       cmp       r8,800
       ja        short M01_L11
       mov       r10,r8
       shr       r10,6
M01_L08:
       vmovdqu   ymm0,ymmword ptr [rdx]
       vmovdqu   ymmword ptr [rcx],ymm0
       vmovdqu   ymm0,ymmword ptr [rdx+20]
       vmovdqu   ymmword ptr [rcx+20],ymm0
       add       rcx,40
       add       rdx,40
       dec       r10
       je        short M01_L10
       jmp       short M01_L08
M01_L09:
       ret
M01_L10:
       and       r8,3F
       cmp       r8,10
       ja        near ptr M01_L02
       vmovupd   xmm0,[rax+0FFF0]
       vmovupd   [r9+0FFF0],xmm0
       jmp       short M01_L09
M01_L11:
       jmp       qword ptr [7FFE2D6E9A08]
; Total bytes of code 270
```

## .NET 7.0.0 (7.0.22.42610), X64 RyuJIT
```assembly
; Bench.ToArray()
       mov       rcx,184E3006358
       mov       rcx,[rcx]
       jmp       qword ptr [7FFE2D6C3678]
; Total bytes of code 19
```
```assembly
; System.Collections.Generic.EnumerableHelpers.ToArray[[System.Byte, System.Private.CoreLib]](System.Collections.Generic.IEnumerable`1<Byte>)
       push      rdi
       push      rsi
       sub       rsp,58
       vzeroupper
       xor       eax,eax
       mov       [rsp+28],rax
       vxorps    xmm4,xmm4,xmm4
       vmovdqa   xmmword ptr [rsp+30],xmm4
       vmovdqa   xmmword ptr [rsp+40],xmm4
       mov       [rsp+50],rax
       mov       rsi,rcx
       mov       rdx,rsi
       mov       rcx,offset MT_System.Collections.Generic.ICollection`1[[System.Byte, System.Private.CoreLib]]
       call      qword ptr [7FFE2D6BB810]
       mov       rdi,rax
       test      rdi,rdi
       je        short M01_L01
       mov       rcx,rdi
       mov       r11,7FFE2D4E03D0
       call      qword ptr [r11]
       test      eax,eax
       jne       short M01_L00
       mov       rax,184E3001F80
       mov       rax,[rax]
       add       rsp,58
       pop       rsi
       pop       rdi
       ret
M01_L00:
       movsxd    rdx,eax
       mov       rcx,offset MT_System.Byte[]
       call      CORINFO_HELP_NEWARR_1_VC
       mov       rsi,rax
       mov       rcx,rdi
       mov       rdx,rsi
       mov       r11,7FFE2D4E03D8
       xor       r8d,r8d
       call      qword ptr [r11]
       mov       rax,rsi
       add       rsp,58
       pop       rsi
       pop       rdi
       ret
M01_L01:
       vxorps    ymm0,ymm0,ymm0
       vmovdqu   ymmword ptr [rsp+28],ymm0
       vmovdqu   xmmword ptr [rsp+48],xmm0
       mov       rcx,184E3001F80
       mov       rcx,[rcx]
       mov       [rsp+30],rcx
       mov       [rsp+28],rcx
       mov       dword ptr [rsp+38],7FFFFFFF
       lea       rcx,[rsp+28]
       mov       rdx,rsi
       call      qword ptr [7FFE2D6C3A68]
       lea       rcx,[rsp+28]
       call      qword ptr [7FFE2D6C3AF8]
       nop
       add       rsp,58
       pop       rsi
       pop       rdi
       ret
; Total bytes of code 240
```
