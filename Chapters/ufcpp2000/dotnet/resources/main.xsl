<?xml version="1.0" encoding="Shift_JIS"?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
<xsl:output method="html" encoding="Shift_JIS"/> 

<xsl:template match="/">
<xsl:apply-templates select="*"/>
</xsl:template>

<xsl:template match="blog">
<div class="blog">
<xsl:apply-templates select="*"/>
</div>
</xsl:template>

<!-- HTMLタグをそのまま表示できるように -->
<xsl:template match="*|text()">
<xsl:copy>
<xsl:for-each select="@*">
<xsl:copy/>
</xsl:for-each>
<xsl:apply-templates/>
</xsl:copy>
</xsl:template>

</xsl:stylesheet>
