using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage(
    "Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores",
    Justification = "Unit test naming follows https://osherove.com/blog/2005/4/3/naming-standards-for-unit-tests.html",
    Scope = "namespaceanddescendants", Target = "~N:TexasHoldemHands.Logic")]

[assembly: SuppressMessage("Style", "IDE0130:Namespace does not match folder structure",
    Justification = "dotnet bug always produces this warning - https://github.com/dotnet/roslyn/issues/55014",
    Scope = "namespace",
    Target = "~N:TexasHoldemHands.Logic")]
