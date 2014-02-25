using System;
using System.Threading.Tasks;
using Acr.Threading;
using Xunit;


namespace Acr.Tests {
    
    public class TimerTests {

        [Fact]
        public async Task PreventOverlap() {
            var src = new TaskCompletionSource<int>();
            var timer = new Timer(TimeSpan.FromSeconds(1), () => src.SetResult(3));
            var r = await src.Task;
            timer.Stop();
            Assert.True(r == 3);
        }
    }
}
