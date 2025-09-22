namespace Ecommerce.Core.Sharing
{
	public class EmailStringBody
	{
		public static string Send(string email,string token,string component,string message)
		{
			string encodeToken=Uri.EscapeDataString(token);
			return $@"
                 <html>
                    <head>
                    <style>
                     .button {{
						background-color: #4CAF50; /* Green */
						border: none;
						color: white;
						padding: 15px 32px;
						text-align: center;
						text-decoration: none;
						display: inline-block;
						font-size: 16px;
						margin: 4px 2px;
						cursor: pointer;
					 }}
                    </style>
                    </head>
					<body>
                       <h1>{message}</h1>
                        <hr>
                        <br>
						<a class="".button"" href=""http://localhost:4200/Account/{component}?email={email}&code={encodeToken}"">
                           {message}
                        </a>
					</body>
                 </html>
                ";
		}
	}
}
