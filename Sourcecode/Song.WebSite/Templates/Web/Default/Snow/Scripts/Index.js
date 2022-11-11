
export default {
  props: {
    formula: {
      type: String
    },
    safe: {
      type: Boolean,
      default: true
    },
    options: {
      type: Object,
      default: function () {
        return {}
      }
    }
  },
  watch: {
    formula () {
      this.renderMathJax()
    }
  },
  mounted () {
    this.renderMathJax()
  },
  methods: {
    renderContent () {
      if (this.safe) {
        this.$refs.mathJaxEl.textContent = this.formula
      } else {
        this.$refs.mathJaxEl.innerHTML = this.formula
      }
    },
 
    renderMathJax () {
      this.renderContent()
      if (window.MathJax) {
        window.MathJax.Hub.Config({
          tex2jax: {
            inlineMath: [['$', '$'], ['(', ')']],
            displayMath: [['$$', '$$'], ['[', ']']],
            processEscapes: true,
            processEnvironments: true
          },
          // Center justify equations in code and markdown cells. Elsewhere
          // we use CSS to left justify single line equations in code cells.
          displayAlign: 'center',
          'HTML-CSS': {
            styles: { '.MathJax_Display': { margin: 0 } },
            linebreaks: { automatic: true }
          },
          ...this.options
        })
        window.MathJax.Hub.Queue([
          'Typeset',
          window.MathJax.Hub,
          this.$refs.mathJaxEl
        ])
      }
    }
  }
}
