namespace backend.Constant
{
    class MailHtmlTemplet{
        public static string CreateHtmlTemplate(string content)
        {
            // Dynamically embed the content into the HTML template
            return $@"
            <div style=""font-family: Arial, sans-serif; background-color: #f9f9f9; margin: 0; padding: 0; color: #333333;"">
                <div style=""max-width: 600px; margin: 20px auto; background-color: #ffffff; border-radius: 8px; overflow: hidden; box-shadow: 0 4px 6px rgba(0, 0, 0, 0.1);"">
                    <!-- Header Section -->
                    <div style=""background-color: #4CAF50; padding: 20px; text-align: center; color: #ffffff;"">
                        <h1 style=""margin: 0; font-size: 24px;"">Placement Preparation</h1>
                    </div>

                    <!-- Content Section -->
                    <div style=""padding: 20px; text-align: center;"">
                        {content}
                    </div>

                    <!-- Footer Section -->
                    <div style=""background-color: #f1f1f1; padding: 10px 20px; text-align: center; font-size: 12px; color: #666666;"">
                        <p>&copy; 2025 Placement Preparation. All Rights Reserved.</p>
                        <p>
                            <a href=""#"" style=""color: #4CAF50; text-decoration: none;"">Privacy Policy</a> | <a href=""#"" style=""color: #4CAF50; text-decoration: none;"">Contact Support</a>
                        </p>
                    </div>
                </div>
            </div>";
        }
    }
}
