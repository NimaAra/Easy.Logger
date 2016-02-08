namespace Easy.Logger.Tests.Unit
{
    using System;
    using NUnit.Framework;
    using Shouldly;

    [TestFixture]
    public sealed class RingBufferTests
    {
        [Test]
        public void When_dequeing_an_empty_buffer()
        {
            var buffer = new RingBuffer<int>(5);

            buffer.Capacity.ShouldBe<uint>(5);
            buffer.Count.ShouldBe<uint>(0);

            int item;
            buffer.TryDequeue(out item).ShouldBeFalse();

            buffer.Capacity.ShouldBe<uint>(5);
            buffer.Count.ShouldBe<uint>(0);
        }

        [Test]
        public void When_enqueing_a_non_lossy_buffer_beyond_its_capacity()
        {
            var buffer = new RingBuffer<int>(3);

            buffer.TryEnqueue(1).ShouldBeTrue();
            buffer.TryEnqueue(2).ShouldBeTrue();
            buffer.TryEnqueue(3).ShouldBeTrue();
            buffer.TryEnqueue(4).ShouldBeFalse();
            buffer.TryEnqueue(5).ShouldBeFalse();

            buffer.Capacity.ShouldBe<uint>(3);
            buffer.Count.ShouldBe<uint>(3);

            int item;
            buffer.TryDequeue(out item).ShouldBeTrue();
            item.ShouldBe(1);

            buffer.TryDequeue(out item).ShouldBeTrue();
            item.ShouldBe(2);

            buffer.TryDequeue(out item).ShouldBeTrue();
            item.ShouldBe(3);

            buffer.Capacity.ShouldBe<uint>(3);
            buffer.Count.ShouldBe<uint>(0);

            buffer.TryDequeue(out item).ShouldBeFalse();
            item.ShouldBe(0);

            buffer.Capacity.ShouldBe<uint>(3);
            buffer.Count.ShouldBe<uint>(0);
        }

        [Test]
        public void When_enqueing_a_lossy_buffer_beyond_its_capacity()
        {
            var buffer = new RingBuffer<int>(3, true);

            buffer.TryEnqueue(1).ShouldBeTrue();
            buffer.TryEnqueue(2).ShouldBeTrue();
            buffer.TryEnqueue(3).ShouldBeTrue();
            buffer.TryEnqueue(4).ShouldBeTrue();
            buffer.TryEnqueue(5).ShouldBeTrue();

            buffer.Capacity.ShouldBe<uint>(3);
            buffer.Count.ShouldBe<uint>(3);

            int item;
            buffer.TryDequeue(out item).ShouldBeTrue();
            item.ShouldBe(3);

            buffer.TryDequeue(out item).ShouldBeTrue();
            item.ShouldBe(4);

            buffer.TryDequeue(out item).ShouldBeTrue();
            item.ShouldBe(5);

            buffer.Capacity.ShouldBe<uint>(3);
            buffer.Count.ShouldBe<uint>(0);

            buffer.TryDequeue(out item).ShouldBeFalse();
            item.ShouldBe(0);

            buffer.Capacity.ShouldBe<uint>(3);
            buffer.Count.ShouldBe<uint>(0);
        }

        [Test]
        public void When_moving_items_to_an_array_of_the_same_size()
        {
            var buffer = new RingBuffer<int>(3);

            buffer.TryEnqueue(1);
            buffer.TryEnqueue(2);
            buffer.TryEnqueue(3);

            buffer.Capacity.ShouldBe<uint>(3);
            buffer.Count.ShouldBe<uint>(3);

            var array = new int[3];
            buffer.MoveTo(array);

            array.Length.ShouldBe(3);
            array[0].ShouldBe(1);
            array[1].ShouldBe(2);
            array[2].ShouldBe(3);

            buffer.Capacity.ShouldBe<uint>(3);
            buffer.Count.ShouldBe<uint>(0);
        }

        [Test]
        public void When_moving_items_from_the_buffer_to_a_smaller_array()
        {
            var buffer = new RingBuffer<int>(5);
            buffer.TryEnqueue(1);
            buffer.TryEnqueue(2);
            buffer.TryEnqueue(3);
            buffer.TryEnqueue(4);
            buffer.TryEnqueue(5);

            buffer.Capacity.ShouldBe<uint>(5);
            buffer.Count.ShouldBe<uint>(5);

            var tmpBuffer = new int[3];

            Should.Throw<ArgumentException>(() => buffer.MoveTo(tmpBuffer))
                .Message.ShouldBe("Offset and length were out of bounds for the array or count is greater than the number of elements from index to the end of the source collection.");

            buffer.Capacity.ShouldBe<uint>(5);
            buffer.Count.ShouldBe<uint>(5);
        }

        [Test]
        public void When_moving_items_from_the_buffer_to_a_larger_array()
        {
            var buffer = new RingBuffer<int>(3);
            buffer.TryEnqueue(1);
            buffer.TryEnqueue(2);
            buffer.TryEnqueue(3);

            buffer.Capacity.ShouldBe<uint>(3);
            buffer.Count.ShouldBe<uint>(3);

            var tmpBuffer = new int[5];
            buffer.MoveTo(tmpBuffer);

            buffer.Capacity.ShouldBe<uint>(3);
            buffer.Count.ShouldBe<uint>(0);

            tmpBuffer.Length.ShouldBe(5);
            tmpBuffer[0].ShouldBe(1);
            tmpBuffer[1].ShouldBe(2);
            tmpBuffer[2].ShouldBe(3);

            tmpBuffer[3].ShouldBe(0);
            tmpBuffer[4].ShouldBe(0);
        }

        [Test]
        public void When_moving_items_from_a_partially_filled_buffer_to_a_larger_array()
        {
            var buffer = new RingBuffer<int>(5);
            buffer.TryEnqueue(1);
            buffer.TryEnqueue(2);
            buffer.TryEnqueue(3);

            buffer.Capacity.ShouldBe<uint>(5);
            buffer.Count.ShouldBe<uint>(3);

            var tmpBuffer = new int[5];
            buffer.MoveTo(tmpBuffer);

            buffer.Capacity.ShouldBe<uint>(5);
            buffer.Count.ShouldBe<uint>(0);

            tmpBuffer.Length.ShouldBe(5);
            tmpBuffer[0].ShouldBe(1);
            tmpBuffer[1].ShouldBe(2);
            tmpBuffer[2].ShouldBe(3);

            tmpBuffer[3].ShouldBe(0);
            tmpBuffer[4].ShouldBe(0);
        }

        [Test]
        public void When_getting_a_full_buffer_as_an_array()
        {
            var buffer = new RingBuffer<int>(5);

            buffer.Capacity.ShouldBe<uint>(5);
            buffer.Count.ShouldBe<uint>(0);

            buffer.TryEnqueue(1);
            buffer.TryEnqueue(2);
            buffer.TryEnqueue(3);
            buffer.TryEnqueue(4);
            buffer.TryEnqueue(5);

            buffer.Count.ShouldBe<uint>(5);

            var tmpArray = buffer.ToArray();

            buffer.Count.ShouldBe<uint>(0);

            tmpArray.ShouldNotBeNull();
            tmpArray.Length.ShouldBe(5);

            tmpArray[0].ShouldBe(1);
            tmpArray[1].ShouldBe(2);
            tmpArray[2].ShouldBe(3);
            tmpArray[3].ShouldBe(4);
            tmpArray[4].ShouldBe(5);
        }

        [Test]
        public void When_getting_a_partial_buffer_as_an_array()
        {
            var buffer = new RingBuffer<int>(5);

            buffer.Capacity.ShouldBe<uint>(5);
            buffer.Count.ShouldBe<uint>(0);

            buffer.TryEnqueue(1);
            buffer.TryEnqueue(2);
            buffer.TryEnqueue(3);

            buffer.Count.ShouldBe<uint>(3);

            var tmpArray = buffer.ToArray();

            buffer.Count.ShouldBe<uint>(0);

            tmpArray.ShouldNotBeNull();
            tmpArray.Length.ShouldBe(3);

            tmpArray[0].ShouldBe(1);
            tmpArray[1].ShouldBe(2);
            tmpArray[2].ShouldBe(3);
        }
    }
}