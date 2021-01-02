<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt" exclude-result-prefixes="msxsl">
    <xsl:output omit-xml-declaration="yes" method="xml" indent="yes" version="1.0"/>

    <xsl:template match="/">
    
            <xsl:apply-templates select="/Root"/>
        
    </xsl:template>
  <xsl:template match="/Root">
    <Root>
      <xsl:variable name="firstname" select="FirstName/text()"/>
      <xsl:variable name="lastname" select="LastName/text()"/>
      <Name>
        <xsl:value-of select="concat($firstname,' ',$lastname)"/>
      </Name>
      <Phone>
        <xsl:variable name="areacode" select="AreadCode/text()"/>
        <xsl:variable name="phone" select="Phone/text()"/>
        <xsl:value-of select="concat('+',$areacode,'-',$phone)"/>
      </Phone>
    </Root>
  </xsl:template>
</xsl:stylesheet>
