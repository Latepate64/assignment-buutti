const PROXY_CONFIG = [
  {
    context: [
      "/book",
    ],
    target: "http://localhost:5195",
    secure: false
  }
]

module.exports = PROXY_CONFIG;
