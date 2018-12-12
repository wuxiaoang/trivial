﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Security;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Trivial.Web
{
    /// <summary>
    /// HTTP web client extension.
    /// </summary>
    public static class HttpClientExtension
    {
        /// <summary>
        /// Serialize the HTTP content into a stream of bytes and copies it to the stream object provided as the stream parameter.
        /// </summary>
        /// <param name="httpContent">The http response content.</param>
        /// <param name="destination">The destination stream to write.</param>
        /// <param name="progress">The progress to report. The value is the length of the stream.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        /// <exception cref="ArgumentNullException">The argument is null.</exception>
        public static Task CopyToAsync(this HttpContent httpContent, Stream destination, IProgress<long> progress, CancellationToken cancellationToken = default)
        {
            return CopyToAsync(httpContent, destination, IO.StreamExtension.DefaultBufferSize, progress, cancellationToken);
        }

        /// <summary>
        /// Serialize the HTTP content into a stream of bytes and copies it to the stream object provided as the stream parameter.
        /// </summary>
        /// <param name="httpContent">The http response content.</param>
        /// <param name="destination">The destination stream to write.</param>
        /// <param name="bufferSize">The size, in bytes, of the buffer. This value must be greater than zero.</param>
        /// <param name="progress">The progress to report. The value is the length of the stream.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        /// <exception cref="ArgumentNullException">The argument is null.</exception>
        public static async Task CopyToAsync(this HttpContent httpContent, Stream destination, int bufferSize, IProgress<long> progress, CancellationToken cancellationToken = default)
        {
            if (httpContent == null) throw new ArgumentNullException(nameof(httpContent));
            if (destination == null) throw new ArgumentNullException(nameof(destination));
            using (var downloadingStream = await httpContent.ReadAsStreamAsync())
            {
                await IO.StreamExtension.CopyToAsync(downloadingStream, destination, bufferSize, progress, cancellationToken);
            }
        }

        /// <summary>
        /// Serialize the HTTP content into a stream of bytes and copies it to the stream object provided as the stream parameter.
        /// </summary>
        /// <param name="httpContent">The http response content.</param>
        /// <param name="fileName">The file name.</param>
        /// <param name="progress">The progress to report, from 0 to 1.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        /// <exception cref="ArgumentNullException">The argument is null.</exception>
        /// <exception cref="ArgumentException">The argument is invalid.</exception>
        /// <exception cref="SecurityException">The caller does not have the required permission.</exception>
        /// <exception cref="IOException">An I/O error.</exception>
        /// <exception cref="DirectoryNotFoundException">The specified path is invalid, such as being on an unmapped drive.</exception>
        /// <exception cref="NotSupportedException">The path of the file refers to a non-file device, such as "con:", "com1:", "lpt1:".</exception>
        public static Task WriteFile(this HttpContent httpContent, string fileName, IProgress<double> progress, CancellationToken cancellationToken = default)
        {
            return WriteFile(httpContent, fileName, IO.StreamExtension.DefaultBufferSize, progress, cancellationToken);
        }

        /// <summary>
        /// Serialize the HTTP content into a stream of bytes and copies it to the stream object provided as the stream parameter.
        /// </summary>
        /// <param name="httpContent">The http response content.</param>
        /// <param name="fileName">The file name.</param>
        /// <param name="bufferSize">The size, in bytes, of the buffer. This value must be greater than zero.</param>
        /// <param name="progress">The progress to report, from 0 to 1.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The file info instance to write.</returns>
        /// <exception cref="ArgumentNullException">The argument is null.</exception>
        /// <exception cref="ArgumentException">The argument is invalid.</exception>
        /// <exception cref="SecurityException">The caller does not have the required permission.</exception>
        /// <exception cref="IOException">An I/O error.</exception>
        /// <exception cref="DirectoryNotFoundException">The specified path is invalid, such as being on an unmapped drive.</exception>
        /// <exception cref="NotSupportedException">The path of the file refers to a non-file device, such as "con:", "com1:", "lpt1:".</exception>
        public static async Task<FileInfo> WriteFile(this HttpContent httpContent, string fileName, int bufferSize, IProgress<double> progress, CancellationToken cancellationToken = default)
        {
            using (var fileStream = new FileStream(fileName, FileMode.Create))
            {
                var relativeProgress = progress != null && httpContent.Headers.ContentLength.HasValue ? new Progress<long>(totalBytes => progress.Report((double)totalBytes / httpContent.Headers.ContentLength.Value)) : null;
                await CopyToAsync(httpContent, fileStream, bufferSize, relativeProgress, cancellationToken);
            }

            progress.Report(1);
            return new FileInfo(fileName);
        }

        /// <summary>
        /// Serialize the HTTP JSON content into an object as the specific type.
        /// </summary>
        /// <typeparam name="T">The type of the result expected.</typeparam>
        /// <param name="httpContent">The http response content.</param>
        /// <returns>The result serialized.</returns>
        /// <exception cref="ArgumentNullException">The argument is null.</exception>
        public static async Task<T> SerializeJsonAsync<T>(this HttpContent httpContent)
        {
            if (httpContent == null) throw new ArgumentNullException(nameof(httpContent));
            using (var stream = await httpContent.ReadAsStreamAsync())
            {
                var serializer = new DataContractJsonSerializer(typeof(T));
                return (T)serializer.ReadObject(stream);
            }
        }

        /// <summary>
        /// Serialize the HTTP XML content into an object as the specific type.
        /// </summary>
        /// <typeparam name="T">The type of the result expected.</typeparam>
        /// <param name="httpContent">The http response content.</param>
        /// <returns>The result serialized.</returns>
        /// <exception cref="ArgumentNullException">The argument is null.</exception>
        public static async Task<T> SerializeXmlAsync<T>(this HttpContent httpContent)
        {
            if (httpContent == null) throw new ArgumentNullException(nameof(httpContent));
            using (var stream = await httpContent.ReadAsStreamAsync())
            {
                var serializer = new DataContractSerializer(typeof(T)); 
                return (T)serializer.ReadObject(stream);
            }
        }

        /// <summary>
        /// Serialize the HTTP content into an object by the specific serializer.
        /// </summary>
        /// <param name="httpContent">The http response content.</param>
        /// <param name="serializer">The serializer to read the object from the stream downloaded.</param>
        /// <returns>The result serialized.</returns>
        /// <exception cref="ArgumentNullException">The argument is null.</exception>
        public static async Task<object> SerializeAsync(this HttpContent httpContent, XmlObjectSerializer serializer)
        {
            if (httpContent == null) throw new ArgumentNullException(nameof(httpContent));
            if (serializer == null) throw new ArgumentNullException(nameof(serializer));
            using (var stream = await httpContent.ReadAsStreamAsync())
            {
                return serializer.ReadObject(stream);
            }
        }

        /// <summary>
        /// Serialize the HTTP content into an object by the specific serializer.
        /// </summary>
        /// <param name="httpContent">The http response content.</param>
        /// <param name="serializer">The serializer to read the object from the stream downloaded.</param>
        /// <returns>The result serialized.</returns>
        /// <exception cref="ArgumentNullException">The argument is null.</exception>
        public static async Task<T> SerializeAsync<T>(this HttpContent httpContent, Func<string, T> serializer)
        {
            if (httpContent == null) throw new ArgumentNullException(nameof(httpContent));
            if (serializer == null) throw new ArgumentNullException(nameof(serializer));
            var str = await httpContent.ReadAsStringAsync();
            return serializer(str);
        }
    }
}