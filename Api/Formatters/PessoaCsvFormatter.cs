using Api.Models;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Api.Formatters
{
    public class PessoaCsvFormatter : TextOutputFormatter
    {
        public PessoaCsvFormatter()
        {
            var textCsvMediaType = MediaTypeHeaderValue.Parse("text/csv");
            var appCsvMediaType = MediaTypeHeaderValue.Parse("application/csv");

            SupportedMediaTypes.Add(textCsvMediaType);
            SupportedMediaTypes.Add(appCsvMediaType);

            SupportedEncodings.Add(Encoding.UTF8);
            SupportedEncodings.Add(Encoding.Unicode);
        }

        protected override bool CanWriteType(Type type)
        {
            if (typeof(Pessoa).IsAssignableFrom(type) || typeof(IEnumerable<Pessoa>).IsAssignableFrom(type))
            {
                return base.CanWriteType(type);
            }

            return false;
        }

        public override Task WriteResponseBodyAsync(OutputFormatterWriteContext context, Encoding selectedEncoding)
        {
            var pessoaCsv = "";

            if (context.Object is IEnumerable<Pessoa>)
            {
                foreach (var pessoa in context.Object as IEnumerable<Pessoa>)
                {
                    pessoaCsv += $"{pessoa.Id};{pessoa.Nome}\n";
                }
            }
            else
            {
                var pessoa = context.Object as Pessoa;

                pessoaCsv = $"{pessoa.Id};{pessoa.Nome}";
            }

            using (var writer = context.WriterFactory(context.HttpContext.Response.Body, selectedEncoding))
            {
                return writer.WriteAsync(pessoaCsv);
            }
        }
    }
}
