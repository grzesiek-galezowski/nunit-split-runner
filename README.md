nunit-split-runner
==================

A wrapper over NUnit console that runs assemblies in batches and produces a single report

Its purpose is to avoid the OutOfMemoryException thrown when NUnit runs many dlls in a single run and does not deallocate created app domains.

I ran into this issue when executing over 4000 tests, so I created this exe.

Some of this code is based on C# port of [NUnitMerger](https://github.com/15below/NUnitMerger). Many thanks to the guys that put together that piece of code.