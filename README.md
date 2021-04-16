# SMLite

[![license](https://img.shields.io/github/license/fawdlstty/SMLite?color=09f)](./LICENSE)
[![cpp](https://img.shields.io/lgtm/grade/cpp/github/fawdlstty/SMLite)](https://lgtm.com/projects/g/fawdlstty/SMLite)
[![nuget](https://img.shields.io/nuget/dt/Fawdlstty.SMLite?label=nuget%20downloads)](https://www.nuget.org/packages/Fawdlstty.SMLite)
[![Total alerts](https://img.shields.io/lgtm/alerts/g/fawdlstty/SMLite.svg?logo=lgtm)](https://lgtm.com/projects/g/fawdlstty/SMLite/alerts/)

<!--
[![csharp](https://img.shields.io/lgtm/grade/csharp/github/fawdlstty/SMLite)](https://lgtm.com/projects/g/fawdlstty/SMLite)
[![python](https://img.shields.io/lgtm/grade/python/github/fawdlstty/SMLite)](https://lgtm.com/projects/g/fawdlstty/SMLite)
[![AppVeyor Build](https://img.shields.io/appveyor/build/fawdlstty/SMLite)](https://ci.appveyor.com/project/fawdlstty/SMLite)
[![Coverage Status](https://coveralls.io/repos/github/fawdlstty/SMLite/badge.svg)](https://coveralls.io/github/fawdlstty/SMLite)
-->

English | [简体中文](./README.zh.md)

State machine library for C, C++, C#, Java, JavaScript, Python, VB.Net

## Support Environments

- C library supported versions
    + `C99` and above
- C++ library supported versions
    + `C++ 11` and above
- C# library (VB.Net) supported runtimes
    + `.Net Framework 4.5` and above
    + `.Net Standard 2.0/2.1`
    + `.Net 5`
- Java library supported versions
    + Java 8
- JavaScript library supported versions
    + ECMAScript 2015
- Python library supported runtimes
    + `Python 3.7`

## Implementation Schedule

|                     |   C   |  C++  | C#/VB.Net |  Java  | JavaScript | Python |
|        :---:        | :---: | :---: |   :---:   | :---: |    :---:    | :---: |
|      Document       |   √   |   √   |     √     |       |             |   √   |
| Sync State Machine  |   √   |   √   |     √     |   √   |      √      |   √   |
|     Multi State     |   -   |   √   |     √     |   √   |             |       |
|     Multi State     |   -   |   √   |     √     |   √   |             |       |
|     Thread Safe     |       |   √   |     √     |   √   |             |       |
| Async State Machine |   -   |       |     √     |   -   |      √      |   √   |
|    Async Cancel     |   -   |   -   |     √     |   -   |      -      |   -   |
|      Serilize       |       |   √   |     √     |       |      √      |       |
|        Test         |   √   |   √   |     √     |   √   |      √      |   √   |

- √ Supported
- \- Can't Supported
- (empty) In Plan

## Tutorials

Accelerated access from mainland Chinese users: [Document Images](https://www.fawdlstty.com/smlite/)

- [C Library Tutorials](docs/chapters/c_en.md)
- [C++ Library Tutorials](docs/chapters/cpp_en.md)
- [C# Library Tutorials](docs/chapters/csharp_en.md)
- Java Library Tutorials
- JavaScript Library Tutorials
- [Python Library Tutorials](docs/chapters/python_en.md)
- [VB.Net Library Tutorials](docs/chapters/vb.net_en.md)

## Depends

The project uses submodules and initializes them if local compilation is required.

```cmd
git clone https://github.com/fawdlstty/SMLite
git submodule update --init --recursive
```

- tstl2cl (Dependency by C Library)
    + https://sourceforge.net/projects/tstl2cl/
    + License: zlib/libpng License
