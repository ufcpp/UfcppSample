<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
<xsl:output method="xml" indent="yes"/>

<xsl:template match="/class">
<Window Class="{@name}">

<xsl:apply-templates select="var"/>

</Window>
</xsl:template>

<xsl:template match="var">
<xsl:element name="{@type}">
	<xsl:attribute name="Name"><xsl:value-of select="@name"/></xsl:attribute>
</xsl:element>
</xsl:template>

</xsl:stylesheet>
