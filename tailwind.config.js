/** @type {import('tailwindcss').Config} */
module.exports = {
    content: ["./Views/**/*.{cshtml,razor}","wwwroot/js/**/*.js" , "./Areas/Views/**/*.cshtml"],
  theme: {
      extend: {
          colors: {
              primary: "#87acec",
              primaryLight: "#a6c1ee"
          },
      },
  },
  plugins: [],
}

