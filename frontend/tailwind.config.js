/** @type {import('tailwindcss').Config} */
export default {
  content: [
    "./index.html",
    "./src/**/*.{js,ts,jsx,tsx}",
  ],
  darkMode: ['class', '[data-theme="dark"]'],
  theme: {
    extend: {
      fontFamily: {
        sans: ['Poppins', 'Inter', 'Segoe UI', 'system-ui', '-apple-system', 'sans-serif'],
      },
      colors: {
        primary: {
          50: '#f0f9ff',
          100: '#e0f2fe',
          200: '#bae6fd',
          300: '#7dd3fc',
          400: '#38bdf8',
          500: '#0ea5e9',
          600: '#0284c7',
          700: '#0369a1',
          800: '#075985',
          900: '#0c4a6e',
        },
        // Navy Dark Mode Colors
        navy: {
          50: '#e8eef7',
          100: '#c5d4e8',
          200: '#8ba3c7',
          300: '#5a7da8',
          400: '#3b5f8a',
          500: '#2a4a6f',
          600: '#1e3a5f',
          700: '#142743',
          800: '#0f1f38',
          900: '#0a1628',
        }
      }
    },
  },
  plugins: [],
}
