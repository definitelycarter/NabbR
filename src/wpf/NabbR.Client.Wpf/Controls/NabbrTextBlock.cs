using FirstFloor.ModernUI.Windows.Controls;
using System;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Navigation;

namespace NabbR.Controls
{
    public class NabbrTextBlock : TextBlock
    {
        private static String ChatMessagePattern = @"(?<roomlink>#\S*)|(?<mention>@\S*)|(?<hyperlink>(?:https?:(?:/{1,3}|[a-z0-9%])|[a-z0-9.\-]+[.](?:com|net|org|edu|gov|mil|aero|asia|biz|cat|coop|info|int|jobs|mobi|museum|name|post|pro|tel|travel|xxx|ac|ad|ae|af|ag|ai|al|am|an|ao|aq|ar|as|at|au|aw|ax|az|ba|bb|bd|be|bf|bg|bh|bi|bj|bm|bn|bo|br|bs|bt|bv|bw|by|bz|ca|cc|cd|cf|cg|ch|ci|ck|cl|cm|cn|co|cr|cs|cu|cv|cx|cy|cz|dd|de|dj|dk|dm|do|dz|ec|ee|eg|eh|er|es|et|eu|fi|fj|fk|fm|fo|fr|ga|gb|gd|ge|gf|gg|gh|gi|gl|gm|gn|gp|gq|gr|gs|gt|gu|gw|gy|hk|hm|hn|hr|ht|hu|id|ie|il|im|in|io|iq|ir|is|it|je|jm|jo|jp|ke|kg|kh|ki|km|kn|kp|kr|kw|ky|kz|la|lb|lc|li|lk|lr|ls|lt|lu|lv|ly|ma|mc|md|me|mg|mh|mk|ml|mm|mn|mo|mp|mq|mr|ms|mt|mu|mv|mw|mx|my|mz|na|nc|ne|nf|ng|ni|nl|no|np|nr|nu|nz|om|pa|pe|pf|pg|ph|pk|pl|pm|pn|pr|ps|pt|pw|py|qa|re|ro|rs|ru|rw|sa|sb|sc|sd|se|sg|sh|si|sj|Ja|sk|sl|sm|sn|so|sr|ss|st|su|sv|sx|sy|sz|tc|td|tf|tg|th|tj|tk|tl|tm|tn|to|tp|tr|tt|tv|tw|tz|ua|ug|uk|us|uy|uz|va|vc|ve|vg|vi|vn|vu|wf|ws|ye|yt|yu|za|zm|zw)/)(?:[^\s()<>{}\[\]]+|\([^\s()]*?\([^\s()]+\)[^\s()]*?\)|\([^\s]+?\))+(?:\([^\s()]*?\([^\s()]+\)[^\s()]*?\)|\([^\s]+?\)|[^\s`!()\[\]{};:'.,<>?«»“”‘’])|(?:(?<!@)[a-z0-9]+(?:[.\-][a-z0-9]+)*[.](?:com|net|org|edu|gov|mil|aero|asia|biz|cat|coop|info|int|jobs|mobi|museum|name|post|pro|tel|travel|xxx|ac|ad|ae|af|ag|ai|al|am|an|ao|aq|ar|as|at|au|aw|ax|az|ba|bb|bd|be|bf|bg|bh|bi|bj|bm|bn|bo|br|bs|bt|bv|bw|by|bz|ca|cc|cd|cf|cg|ch|ci|ck|cl|cm|cn|co|cr|cs|cu|cv|cx|cy|cz|dd|de|dj|dk|dm|do|dz|ec|ee|eg|eh|er|es|et|eu|fi|fj|fk|fm|fo|fr|ga|gb|gd|ge|gf|gg|gh|gi|gl|gm|gn|gp|gq|gr|gs|gt|gu|gw|gy|hk|hm|hn|hr|ht|hu|id|ie|il|im|in|io|iq|ir|is|it|je|jm|jo|jp|ke|kg|kh|ki|km|kn|kp|kr|kw|ky|kz|la|lb|lc|li|lk|lr|ls|lt|lu|lv|ly|ma|mc|md|me|mg|mh|mk|ml|mm|mn|mo|mp|mq|mr|ms|mt|mu|mv|mw|mx|my|mz|na|nc|ne|nf|ng|ni|nl|no|np|nr|nu|nz|om|pa|pe|pf|pg|ph|pk|pl|pm|pn|pr|ps|pt|pw|py|qa|re|ro|rs|ru|rw|sa|sb|sc|sd|se|sg|sh|si|sj|Ja|sk|sl|sm|sn|so|sr|ss|st|su|sv|sx|sy|sz|tc|td|tf|tg|th|tj|tk|tl|tm|tn|to|tp|tr|tt|tv|tw|tz|ua|ug|uk|us|uy|uz|va|vc|ve|vg|vi|vn|vu|wf|ws|ye|yt|yu|za|zm|zw)\b/?(?!@)))";

        private readonly static Regex regex = new Regex(ChatMessagePattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);

        static NabbrTextBlock()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(NabbrTextBlock), new FrameworkPropertyMetadata(typeof(NabbrTextBlock)));
        }

        public String MessageContent
        {
            get { return (String)GetValue(MessageContentProperty); }
            set { SetValue(MessageContentProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MessageContent.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MessageContentProperty =
            DependencyProperty.Register("MessageContent", typeof(String), typeof(NabbrTextBlock), new PropertyMetadata(OnMessageContentChanged));

        private static void OnMessageContentChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            NabbrTextBlock message = (NabbrTextBlock)d;
            message.Rebuild();
        }


        private void Rebuild()
        {
            this.Inlines.Clear();

            if (String.IsNullOrEmpty(this.MessageContent))
            {
                return;
            }

            if (this.MessageContent.IndexOf('\n') == -1)
            {
                MatchCollection matches = regex.Matches(this.MessageContent);

                if (matches.Count == 0)
                {
                    this.Inlines.Add(new Run(this.MessageContent));
                }
                else
                {
                    Int32 start = 0;
                    foreach (Match match in matches)
                    {
                        if (match.Index > start)
                        {
                            this.Inlines.Add(new Run(this.MessageContent.Substring(start, (match.Index - start))));
                            start = match.Index;
                        }

                        this.AddMatch(match);
                        start = start + match.Length;
                    }

                    if (start < this.MessageContent.Length - 1)
                    {
                        this.Inlines.Add(new Run(this.MessageContent.Substring(start, this.MessageContent.Length - start)));
                    }
                }
            }
            else
            {
                this.Inlines.Add(new Run(this.MessageContent));
            }
        }

        private void AddMatch(Match match)
        {
            String value = match.Value;

            if (match.Groups["hyperlink"].Success)
            {
                var text = new Run(value);
                Hyperlink externalLink = new Hyperlink(text);
                
                UriBuilder builder = new UriBuilder(value);
                externalLink.NavigateUri = builder.Uri;
                externalLink.RequestNavigate += OnRequestExternalNavigation;

                this.Inlines.Add(externalLink);
            }
            else if (match.Groups["roomlink"].Success)
            {
                //this.Inlines.Add(new Hyperlink(new Run(value) { FontWeight = FontWeights.Bold }));
                this.Inlines.Add(new Run(value));
            }
            else if (match.Groups["mention"].Success)
            {
                var run = new Run(value);
                run.FontWeight = FontWeights.Bold;
                this.Inlines.Add(new Italic(run));
            }
        }

        private void OnRequestExternalNavigation(Object sender, RequestNavigateEventArgs e)
        {
            Process.Start(e.Uri.OriginalString);
        }
    }
}
