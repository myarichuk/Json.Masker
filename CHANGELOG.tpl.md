# Changelog

All notable changes to this project will be documented in this file.

{{ range .Versions }}
## [{{ .Tag.Name }}] - {{ datetime "2006-01-02" .Tag.Date }}
{{- if .CommitGroups -}}
{{ range .CommitGroups -}}
### {{ .Title }}
{{ range .Commits -}}
- {{ if .Scope }}**{{ .Scope }}:** {{ end }}{{ .Subject }}
  {{ end }}
  {{ end -}}
  {{- end -}}
  {{ if .Notes -}}
### BREAKING CHANGES
{{ range .Notes }}
- {{ .Body }}
  {{ end }}
  {{ end -}}
  {{ end -}}
