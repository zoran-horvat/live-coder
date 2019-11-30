using System;
using System.IO;
using LiveCoder.Common.Optional;
using LiveCoder.Scripting.Execution;
using LiveCoder.Scripting.Parsing.Instructions;
using Xunit;

namespace LiveCoder.Scripting.Tests.Parsing.Instructions
{
    public class SelectGlobalScopeExecuteTests
    {
        private class GlobalContext : IContext
        {
            private Option<ISolution> PresetSolution { get; }

            public GlobalContext()
            {
                this.PresetSolution = None.Value;
            }

            public GlobalContext(ISolution solution)
            {
                this.PresetSolution = Option.Of(solution);
            }

            public ISolution Solution =>
                this.PresetSolution.Reduce(() => throw new InvalidOperationException());
        }

        private class LocalScope : ISolution
        {
            public Option<FileInfo> File => throw new InvalidOperationException();
        }

        [Fact]
        public void ReceivesNoneLocalScope_ReturnsGlobalScope() =>
            AssertGlobalScopeReturned(new GlobalContext(), None.Value);

        [Fact]
        public void ReceivesNoneLocalScope_ReturnsSomeOfGlobalContext() =>
            this.AssertSomeOfGlobalContextReturned(new GlobalContext(), None.Value);

        [Fact]
        public void ReceivesSomeLocalScope_ReturnsGlobalScope() =>
            this.AssertGlobalScopeReturned(new LocalScope());

        [Fact]
        public void ReceivesSomeLocalScope_ReturnsSomeOfGlobalContext() =>
            this.AssertSomeOfGlobalContextReturned(new LocalScope());

        private void AssertGlobalScopeReturned(ISolution localScope) =>
            this.AssertGlobalScopeReturned(new GlobalContext(localScope), Option.Of(localScope));

        private void AssertGlobalScopeReturned(IContext globalScope, Option<object> localScope) =>
            Assert.Same(globalScope, this.UnpackResult(globalScope, localScope));

        private void AssertSomeOfGlobalContextReturned(ISolution localScope) =>
            this.AssertSomeOfGlobalContextReturned(new GlobalContext(localScope), Option.Of(localScope));

        private void AssertSomeOfGlobalContextReturned(IContext globalScope, Option<object> localScope) =>
            Assert.True(
                this.Execute(globalScope, localScope) is Some<object> some && 
                some.Content is GlobalContext);

        private object UnpackResult(IContext globalScope, Option<object> localScope) => 
            this.Execute(globalScope, localScope).Reduce(() => throw new InvalidOperationException());

        private Option<object> Execute(IContext globalScope, Option<object> localScope) =>
            new SelectGlobalScope().Execute(globalScope, localScope);
    }
}
