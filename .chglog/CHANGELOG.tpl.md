# Changelog

All notable changes to this project will be documented in this file.

{{- range .Versions }}
## {{- if .Tag.Previous -}}
[{{ .Tag.Name }}]({{ $.Info.RepositoryURL }}/compare/{{ .Tag.Previous.Name }}...{{ .Tag.Name }}) - {{ datetime "2006-01-02" .Tag.Date }}
{{- else -}}
{{ .Tag.Name }} - {{ datetime "2006-01-02" .Tag.Date }}
{{- end }}

{{- if .CommitGroups }}
{{- range .CommitGroups }}
### {{ .Title }}
{{- range .Commits }}
- {{- if .Scope }}**{{ .Scope }}:** {{ end -}}{{ .Subject }}
  {{- end }}
  {{- end }}
  {{- end }}

{{- if .RevertCommits }}
### Reverts
{{- range .RevertCommits }}
- {{ .Header }}
  {{- end }}
  {{- end }}

{{- if .MergeCommits }}
### Merges
{{- range .MergeCommits }}
- {{ .Header }}
  {{- end }}
  {{- end }}

{{- if .NoteGroups }}
{{- range .NoteGroups }}
### {{ .Title }}
{{- range .Notes }}
- {{ .Body }}
  {{- end }}
  {{- end }}
  {{- end }}

{{ end -}}
