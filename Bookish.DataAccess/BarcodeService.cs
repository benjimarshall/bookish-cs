using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using BarcodeLib;
using Bookish.DataAccess.Records;

namespace Bookish.DataAccess
{
    public interface IBarcodeService
    {
        public IEnumerable<AddedBook> AddBook(Book book, int quantity);
    }

    public class BarcodeService : IBarcodeService
    {
        private readonly IBookishService bookishService;

        public BarcodeService(IBookishService bookishService)
        {
            this.bookishService = bookishService;
        }

        public IEnumerable<AddedBook> AddBook(Book book, int quantity)
        {
            var bookIds = bookishService.AddBook(book, quantity);

            var barcode = new Barcode();

            return bookIds.Select(bookId =>
            {
                var barcodeImage = barcode.Encode(TYPE.CODE39, bookId.ToString(), Color.Black, Color.White, 270, 170);

                // Put the barcode in a frame in its image
                var framedBarcode = new Bitmap(300, 200);
                using var g = Graphics.FromImage(framedBarcode);

                // Set the whole image to be white
                g.Clear(Color.White);

                // Draw a 5px black frame around the edges
                var framePen = new Pen(Color.Black, 5) { Alignment = System.Drawing.Drawing2D.PenAlignment.Inset };
                g.DrawRectangle(framePen, new Rectangle(0, 0, 300, 200));

                // Insert the barcode image starting at (15, 15)px, where the first 5px are border, and the next 10px
                // are white padding around the barcode image
                g.DrawImage(barcodeImage, 15, 15);

                // Convert the barcode image into a string base64 encoded in base-64, to be inlined by the website
                using var stream = new MemoryStream();
                framedBarcode.Save(stream, ImageFormat.Png);
                var imageBytes = stream.ToArray();
                return new AddedBook(bookId, $"data:image/png;base64,{Convert.ToBase64String(imageBytes)}");
            });
        }
    }
}
